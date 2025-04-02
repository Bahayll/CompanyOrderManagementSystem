using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.ProductCategories;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductCategoryRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.ProductCategories;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using MediatR;


namespace CompanyOrderManagement.Application.Features.ProductCategories.Queries.GetById
{
    public class GetByIdProductCategoryQueryHandler : IRequestHandler<GetByIdProductCategoryQueryRequest, ApiResponse<GetByIdProductCategoryQueryResponse>>
    {
        private readonly IProductCategoryUnitOfWork _productCategoryUnitOfWork;
        private readonly IProductCategoryBusinessRules _productCategoryBusinessRules;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly ICacheService _cacheService;

        public GetByIdProductCategoryQueryHandler(IProductCategoryUnitOfWork productCategoryUnitOfWork, IProductCategoryBusinessRules productCategoryBusinessRules, IMapper mapper, ILoggerService logger, ICacheService cacheService)
        {
            _productCategoryUnitOfWork = productCategoryUnitOfWork;
            _productCategoryBusinessRules = productCategoryBusinessRules;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<GetByIdProductCategoryQueryResponse>> Handle(GetByIdProductCategoryQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _productCategoryBusinessRules.EnsureProductCategoryIdExists(request.Id);

                var cacheKey = CacheKeys.ProductCategoryById(request.Id);
                var cachedCategory = _cacheService.Get<ProductCategory>(cacheKey);

                if (cachedCategory != null)
                {
                    _logger.Info(ProductCategoryLogMessage.CacheSuccessfullyFetchedById(request.Id));
                    var responseFromCache = _mapper.Map<GetByIdProductCategoryQueryResponse>(cachedCategory);
                    return ApiResponse<GetByIdProductCategoryQueryResponse>.Success(responseFromCache);
                }

                var productCategory = await _productCategoryUnitOfWork.GetReadRepository.GetByIdAsync(request.Id);

                _cacheService.Set(cacheKey,productCategory,TimeSpan.FromHours(1));

                var response = _mapper.Map<GetByIdProductCategoryQueryResponse>(productCategory);
                _logger.Info(ProductCategoryLogMessage.SuccessfullyFetchedById(request.Id));
                return ApiResponse<GetByIdProductCategoryQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ProductCategoryLogMessage.FailedToFetchById(request.Id,ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("FetchByIdError", ex.Message) }, true);
                return ApiResponse<GetByIdProductCategoryQueryResponse>.Fail(error);
            }
        }
    }
}
