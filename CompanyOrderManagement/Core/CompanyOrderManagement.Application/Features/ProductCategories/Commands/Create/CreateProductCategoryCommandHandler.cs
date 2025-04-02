using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.ProductCategories;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductCategoryRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.ProductCategories;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using MediatR;

namespace CompanyOrderManagement.Application.Features.ProductCategories.Commands.Create
{
    public class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommandRequest, ApiResponse<CreateProductCategoryCommandResponse>>
    {
        public readonly IProductCategoryUnitOfWork _productCategoryUnitOfWork;
        private readonly IProductCategoryBusinessRules _productCategoryBusinessRules;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly ICacheService _cacheService;

        public CreateProductCategoryCommandHandler(IProductCategoryUnitOfWork productCategoryUnitOfWork, IProductCategoryBusinessRules productCategoryBusinessRules, IMapper mapper, ILoggerService logger, ICacheService cacheService)
        {
            _productCategoryUnitOfWork = productCategoryUnitOfWork;
            _productCategoryBusinessRules = productCategoryBusinessRules;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<CreateProductCategoryCommandResponse>> Handle(CreateProductCategoryCommandRequest request, CancellationToken cancellationToken)
        {
  
            try
            {
                await _productCategoryBusinessRules.EnsureUniqueCategoryNameAsync(request.Name);

                var productCategory = _mapper.Map<ProductCategory>(request);
                await _productCategoryUnitOfWork.GetWriteRepository.AddAsync(productCategory);
                await _productCategoryUnitOfWork.SaveAsync();
                _logger.Info(ProductCategoryLogMessage.SuccessfullyCreated(request.Name));

                var cacheKey = CacheKeys.AllProductCategories;
                var categoriesInCache = _cacheService.Get<List<ProductCategory>>(cacheKey) ?? new List<ProductCategory>();

                categoriesInCache.Add(productCategory);
                _cacheService.Set(cacheKey, categoriesInCache, TimeSpan.FromHours(1));

                _logger.Info(ProductCategoryLogMessage.CacheUpdated(cacheKey,request.Name));

                var response = _mapper.Map<CreateProductCategoryCommandResponse>(productCategory);
                return ApiResponse<CreateProductCategoryCommandResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ProductCategoryLogMessage.FailedToCreate(ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("CreateError", ex.Message) }, true);
                return ApiResponse<CreateProductCategoryCommandResponse>.Fail(error);
            }
        }
    }
}
