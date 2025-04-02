using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Products;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Products;
using CompanyOrderManagement.Domain.Entities;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Products.Commands.Create
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, ApiResponse<CreateProductCommandResponse>>
    {
        private readonly  IProductUnitOfWork _productUnitOfWork;
        private readonly IProductBusinessRules _productBusinessRules;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public CreateProductCommandHandler(IProductUnitOfWork productUnitOfWork, IProductBusinessRules productBusinessRules, IMapper mapper, ILoggerService logger)
        {
            _productUnitOfWork = productUnitOfWork;
            _productBusinessRules = productBusinessRules;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<CreateProductCommandResponse>> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _productBusinessRules.EnsureUniqueProductNameAsync(request.Name);
                await _productBusinessRules.EnsureCompanyExistsAsync(request.CompanyId);
                await _productBusinessRules.EnsureCategoryExistsAsync(request.ProductCategoryId);

                var products = _mapper.Map<Product>(request);
                await _productUnitOfWork.GetWriteRepository.AddAsync(products);
                await _productUnitOfWork.SaveAsync();
                _logger.Info(ProductLogMessage.SuccessfullyCreated(request.Name));

                var response = _mapper.Map<CreateProductCommandResponse>(products);

                return ApiResponse<CreateProductCommandResponse>.Success(response);
            }
             catch (Exception ex)
            {
                _logger.Error(ProductLogMessage.FailedToCreate(ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("CreateError", ex.Message) }, true);
                return ApiResponse<CreateProductCommandResponse>.Fail(error);
            }
           
        }
    }
}
