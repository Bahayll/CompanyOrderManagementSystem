using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Companies;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Companies;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Companies.Commands.Update
{
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommandRequest, ApiResponse<UpdateCompanyCommandResponse>>
    {
        private readonly ICompanyUnitOfWork _companyUnitOfWork;
        private readonly ICompanyBusinessRules _companyBusinessRules;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly ICacheService _cacheService;

        public UpdateCompanyCommandHandler(ICompanyUnitOfWork companyUnitOfWork, ICompanyBusinessRules companyBusinessRules, IMapper mapper, ILoggerService logger, ICacheService cacheService)
        {
            _companyUnitOfWork = companyUnitOfWork;
            _companyBusinessRules = companyBusinessRules;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<UpdateCompanyCommandResponse>> Handle(UpdateCompanyCommandRequest request, CancellationToken cancellationToken)
        {
           
            try
            {
                await _companyBusinessRules.EnsureCompanyIdExists(request.Id);
                await _companyBusinessRules.CompanyNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id);
                await _companyBusinessRules.EnsureCompanyDetailsAreUpdated(request);

                var company = await _companyUnitOfWork.GetReadRepository.GetByIdAsync(request.Id);
                _mapper.Map(request, company);
                _companyUnitOfWork.GetWriteRepository.Update(company);
                await _companyUnitOfWork.SaveAsync();
                _logger.Info(CompanyLogMessage.SuccessfullyUpdated(request.Id));


                var cacheKey = CacheKeys.AllCompanies;
                var cachedCompanies = _cacheService.Get<List<Company>>(cacheKey) ?? new List<Company>();

                var companyInCache = cachedCompanies.FirstOrDefault(c => c.Id == request.Id);
                if (companyInCache != null)
                {
                    _mapper.Map(company, companyInCache);
                    _logger.Info(CompanyLogMessage.CacheUpdatedForCompany(request.Id));
                }
                else
                {
                    cachedCompanies.Add(company);
                    _logger.Info(CompanyLogMessage.CacheAddedForCompany(request.Id));
                }

                _cacheService.Set(cacheKey, cachedCompanies, TimeSpan.FromHours(1));
                _logger.Info(CompanyLogMessage.CacheUpdated(cacheKey, company.Name));


                var response = _mapper.Map<UpdateCompanyCommandResponse>(company);
                return ApiResponse<UpdateCompanyCommandResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.Error(CompanyLogMessage.FailedToUpdate(request.Id,ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("UpdateError", ex.Message) }, true);
                return ApiResponse<UpdateCompanyCommandResponse>.Fail(error);
            }
                
        }
    }
}
