using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Companies;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Companies;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Companies.Commands.Create
{
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommandRequest, ApiResponse<CreateCompanyCommandResponse>>
    { 
        private readonly ICompanyUnitOfWork _companyUnitOfWork;
        private readonly ICompanyBusinessRules _companyBusinessRules;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly ICacheService _cacheService;

        public CreateCompanyCommandHandler(ICompanyUnitOfWork companyUnitOfWork, ICompanyBusinessRules companyBusinessRules, IMapper mapper, ILoggerService logger, ICacheService cacheService)
        {
            _companyUnitOfWork = companyUnitOfWork;
            _companyBusinessRules = companyBusinessRules;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<CreateCompanyCommandResponse>> Handle(CreateCompanyCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _companyBusinessRules.EnsureUniqueCompanyNameAsync(request.Name);

                var company = _mapper.Map<Company>(request);
                await _companyUnitOfWork.GetWriteRepository.AddAsync(company);
                await _companyUnitOfWork.SaveAsync();

                _logger.Info(CompanyLogMessage.SuccessfullyCreated(request.Name));

                var cacheKey = CacheKeys.AllCompanies;
                var companiesInCache = _cacheService.Get<List<Company>>(cacheKey) ?? new List<Company>();
                companiesInCache.Add(company);
                _cacheService.Set(cacheKey, companiesInCache, TimeSpan.FromHours(1));

                _logger.Info(CompanyLogMessage.CacheUpdated(cacheKey,request.Name));

                var response = _mapper.Map<CreateCompanyCommandResponse>(company);
                return ApiResponse<CreateCompanyCommandResponse>.Success(response);
            }   
            catch (Exception ex)
            {
                _logger.Error(CompanyLogMessage.FailedToCreate(ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("CreateError", ex.Message) }, true);
                return ApiResponse<CreateCompanyCommandResponse>.Fail(error);
            }


        }
    }
}
