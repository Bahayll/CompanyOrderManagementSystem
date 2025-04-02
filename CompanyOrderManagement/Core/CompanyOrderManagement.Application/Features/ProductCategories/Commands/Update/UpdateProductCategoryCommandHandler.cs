using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.ProductCategories;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductCategoryRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.ProductCategories;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using MediatR;

namespace CompanyOrderManagement.Application.Features.ProductCategories.Commands.Update
{
    public class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommandRequest, ApiResponse<UpdateProductCategoryCommandResponse>>
    {
        private readonly IProductCategoryUnitOfWork _productCategoryUnitOfWork;
        private readonly IProductCategoryBusinessRules _productCategoryBusinessRules;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly ICacheService _cacheService;

        public UpdateProductCategoryCommandHandler(IProductCategoryUnitOfWork productCategoryUnitOfWork, IProductCategoryBusinessRules productCategoryBusinessRules, IMapper mapper, ILoggerService logger, ICacheService cacheService)
        {
            _productCategoryUnitOfWork = productCategoryUnitOfWork;
            _productCategoryBusinessRules = productCategoryBusinessRules;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<UpdateProductCategoryCommandResponse>> Handle(UpdateProductCategoryCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _productCategoryBusinessRules.EnsureProductCategoryIdExists(request.Id);
                await _productCategoryBusinessRules.ProductCategoryNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id);
                await _productCategoryBusinessRules.EnsureProductDetailsAreUpdated(request);

                var productCategory = await _productCategoryUnitOfWork.GetReadRepository.GetByIdAsync(request.Id);
                _mapper.Map(request, productCategory);
                _productCategoryUnitOfWork.GetWriteRepository.Update(productCategory);
                await _productCategoryUnitOfWork.SaveAsync();
                _logger.Info(ProductCategoryLogMessage.SuccessfullyUpdated(request.Id));

                var cacheKey = CacheKeys.AllProductCategories;
                var categoriesInCache = _cacheService.Get<List<ProductCategory>>(cacheKey) ?? new List<ProductCategory>();

                var categoryInCache = categoriesInCache.FirstOrDefault(c => c.Id == request.Id);
                if (categoryInCache != null)
                {
                    _mapper.Map(productCategory, categoryInCache);
                    _logger.Info(ProductCategoryLogMessage.CacheUpdatedForCategory(request.Id));
                }
                else
                {
                    categoriesInCache.Add(productCategory);
                    _logger.Info(ProductCategoryLogMessage.CacheAddedForCategory(request.Id));
                }

                _cacheService.Set(cacheKey,categoriesInCache,TimeSpan.FromHours(1));
                _logger.Info(ProductCategoryLogMessage.CacheUpdated(cacheKey,request.Name));


                var response = _mapper.Map<UpdateProductCategoryCommandResponse>(request);
                return ApiResponse<UpdateProductCategoryCommandResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ProductCategoryLogMessage.FailedToDelete(request.Id, ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("UpdateError", ex.Message) }, true);
                return ApiResponse<UpdateProductCategoryCommandResponse>.Fail(error);
            }
        }
    }
}
