using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Companies;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Services.Cache;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Companies.Queries.GetAll
{
    public class GetAllCompanyQueryHandler : IRequestHandler<GetAllCompanyQueryRequest, ApiResponse<List<GetAllCompanyQueryResponse>>>
    {
        private readonly ICompanyUnitOfWork _companyUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly ICacheService _cacheService;

        public GetAllCompanyQueryHandler(ICompanyUnitOfWork companyUnitOfWork, IMapper mapper, ILoggerService logger, ICacheService cacheService)
        {
            _companyUnitOfWork = companyUnitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<List<GetAllCompanyQueryResponse>>> Handle(GetAllCompanyQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = CacheKeys.GetAllCompanies;
                var cachedCompanies = _cacheService.Get<List<GetAllCompanyQueryResponse>>(cacheKey);

                if (cachedCompanies != null)
                {
                    _logger.Info(CompanyLogMessage.CacheRetrievedFromCache(cachedCompanies.Count));
                    return ApiResponse<List<GetAllCompanyQueryResponse>>.Success(cachedCompanies);
                }

                _logger.Info(CompanyLogMessage.CacheNotFoundFetchingFromDatabase());
                var companies =  _companyUnitOfWork.GetReadRepository.GetAll().ToList();
                var response = _mapper.Map<List<GetAllCompanyQueryResponse>>(companies);

                _cacheService.Set(cacheKey, response, TimeSpan.FromHours(1));

                _logger.Info(CompanyLogMessage.SuccessfullyFetchedAll(response.Count));
                return ApiResponse<List<GetAllCompanyQueryResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.Error(CompanyLogMessage.FailedToFetchAll(ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("FetchAllError", ex.Message) }, true);
                return ApiResponse<List<GetAllCompanyQueryResponse>>.Fail(error);
            }



        }
    }
}
