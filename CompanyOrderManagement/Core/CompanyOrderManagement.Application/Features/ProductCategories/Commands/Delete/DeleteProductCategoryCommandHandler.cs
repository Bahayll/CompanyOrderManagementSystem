using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.ProductCategories;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductCategoryRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.ProductCategories;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using MediatR;


namespace CompanyOrderManagement.Application.Features.ProductCategories.Commands.Delete
{
    public class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommandRequest, NoContentResponse>
    {
        private readonly IProductCategoryUnitOfWork _productCategoryUnitOfWork;
        private readonly IProductCategoryBusinessRules _productCategoryBusinessRules;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly ICacheService _cacheService;

        public DeleteProductCategoryCommandHandler(IProductCategoryUnitOfWork productCategoryUnitOfWork, IProductCategoryBusinessRules productCategoryBusinessRules, IMapper mapper, ILoggerService logger, ICacheService cacheService)
        {
            _productCategoryUnitOfWork = productCategoryUnitOfWork;
            _productCategoryBusinessRules = productCategoryBusinessRules;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<NoContentResponse> Handle(DeleteProductCategoryCommandRequest request, CancellationToken cancellationToken)
        {

            try
            {
                await _productCategoryBusinessRules.EnsureProductCategoryIdExists(request.Id);

                await _productCategoryUnitOfWork.GetWriteRepository.RemoveAsync(request.Id);
                await _productCategoryUnitOfWork.SaveAsync();
                _logger.Info(ProductCategoryLogMessage.SuccessfullyDeleted(request.Id));

                var cacheKey = CacheKeys.AllProductCategories;
                var categoriesInCache = _cacheService.Get<List<ProductCategory>>(cacheKey);
                if (categoriesInCache != null)
                {
                    var categoryToRemove = categoriesInCache.FirstOrDefault(c => c.Id == request.Id);
                    if (categoryToRemove != null)
                    {
                        categoriesInCache.Remove(categoryToRemove);
                        _cacheService.Set(cacheKey, categoriesInCache, TimeSpan.FromHours(1));
                        _logger.Info(ProductCategoryLogMessage.CacheRemoved(request.Id));
                    }
                }

                return NoContentResponse.Success();
            }
            catch (Exception ex) 
            {
                _logger.Error(ProductCategoryLogMessage.FailedToDelete(request.Id, ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("DeleteError", ex.Message) }, true);
                return NoContentResponse.Fail(error);
            }

            
        }
    }
}
