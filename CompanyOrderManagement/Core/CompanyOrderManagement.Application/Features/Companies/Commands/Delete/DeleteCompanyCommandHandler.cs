using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Companies;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Companies;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using MediatR;

namespace CompanyOrderManagement.Application.Features.Companies.Commands.Delete
{
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommandRequest, NoContentResponse>
    {
        private readonly ICompanyUnitOfWork _companyUnitOfWork;
        private readonly ICompanyBusinessRules _companyBusinessRules;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly ICacheService _cacheService;


        public DeleteCompanyCommandHandler(ICompanyUnitOfWork companyUnitOfWork, ICompanyBusinessRules companyBusinessRules, IMapper mapper, ILoggerService logger, ICacheService cacheService)
        {
            _companyUnitOfWork = companyUnitOfWork;
            _companyBusinessRules = companyBusinessRules;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<NoContentResponse> Handle(DeleteCompanyCommandRequest request, CancellationToken cancellationToken)
        {

            try
            {
                await _companyBusinessRules.EnsureCompanyIdExists(request.Id);

                await _companyUnitOfWork.GetWriteRepository.RemoveAsync(request.Id);
                await _companyUnitOfWork.SaveAsync();
                _logger.Info(CompanyLogMessage.SuccessfullyDeleted(request.Id));


                var cacheKey = CacheKeys.AllCompanies;
                var companiesInCache = _cacheService.Get<List<Company>>(cacheKey) ?? new List<Company>();
                var companyToRemove = companiesInCache.FirstOrDefault(c => c.Id == request.Id);
                if (companyToRemove != null)
                {
                    companiesInCache.Remove(companyToRemove);
                    _cacheService.Set(cacheKey, companiesInCache, TimeSpan.FromHours(1));
                    _logger.Info(CompanyLogMessage.CacheRemoved(request.Id));
                }

                return NoContentResponse.Success();
            }
            catch (Exception ex)
            {
                _logger.Error(CompanyLogMessage.FailedToDelete(request.Id,ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("DeleteError", ex.Message) }, true);
                return NoContentResponse.Fail(error);
            }
           
        }
    }
}
