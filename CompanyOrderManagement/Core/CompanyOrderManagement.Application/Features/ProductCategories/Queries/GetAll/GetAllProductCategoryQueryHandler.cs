using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.ProductCategories;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductCategoryRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Services.Cache;
using MediatR;



namespace CompanyOrderManagement.Application.Features.ProductCategories.Queries.GetAll
{
    public class GetAllProductCategoryQueryHandler : IRequestHandler<GetAllProductCategoryQueryRequest, ApiResponse<List<GetAllProductCategoryQueryResponse>>>
    {
        private readonly IProductCategoryUnitOfWork _productCategoryUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly ICacheService _cacheService;

        public GetAllProductCategoryQueryHandler(IProductCategoryUnitOfWork unitOfWork, IMapper mapper, ILoggerService logger, ICacheService cacheService)
        {
            _productCategoryUnitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<List<GetAllProductCategoryQueryResponse>>> Handle(GetAllProductCategoryQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var cacheKey = CacheKeys.GetAllProductCategories;
                var cachedCategories = _cacheService.Get<List<GetAllProductCategoryQueryResponse>>(cacheKey);

                if (cachedCategories != null)
                {
                    _logger.Info(ProductCategoryLogMessage.CacheRetrievedFromCache(cachedCategories.Count));
                    return ApiResponse<List<GetAllProductCategoryQueryResponse>>.Success(cachedCategories);

                }
                _logger.Info(ProductCategoryLogMessage.CacheNotFoundFetchingFromDatabase());

                var ProductCategories = _productCategoryUnitOfWork.GetReadRepository.GetAll().ToList();
                var response = _mapper.Map<List<GetAllProductCategoryQueryResponse>>(ProductCategories);

                _cacheService.Set(cacheKey,response,TimeSpan.FromHours(1));


                _logger.Info(ProductCategoryLogMessage.SuccessfullyFetchedAll(response.Count));
                return ApiResponse<List<GetAllProductCategoryQueryResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ProductCategoryLogMessage.FailedToFetchAll(ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("FetchAllError", ex.Message) }, true);
                return ApiResponse<List<GetAllProductCategoryQueryResponse>>.Fail(error);
            }
          
           
           
        }
    }
}
