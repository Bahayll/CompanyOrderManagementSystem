using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Products;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Products;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Products.Commands.Update
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, ApiResponse<UpdateProductCommandResponse>>
    {
        private readonly IProductUnitOfWork _productUnitOfWork;
        private readonly IProductBusinessRules _productBusinessRules;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public UpdateProductCommandHandler(IProductUnitOfWork productUnitOfWork, IProductBusinessRules productBusinessRules, IMapper mapper, ILoggerService logger)
        {
            _productUnitOfWork = productUnitOfWork;
            _productBusinessRules = productBusinessRules;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<UpdateProductCommandResponse>> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {


            try
            {
                await _productBusinessRules.EnsureProductIdExists(request.Id);
                await _productBusinessRules.ProductNameCanNotBeDuplicatedWhenUpdated(request.Name, request.Id);
                await _productBusinessRules.EnsureProductDetailsAreUpdated(request);

                var product = await _productUnitOfWork.GetReadRepository.GetByIdAsync(request.Id);
                _mapper.Map(request, product);
                _productUnitOfWork.GetWriteRepository.Update(product);
                await _productUnitOfWork.SaveAsync();
                _logger.Info(ProductLogMessage.SuccessfullyUpdated(request.Id));


                var response = _mapper.Map<UpdateProductCommandResponse>(product);
                return ApiResponse<UpdateProductCommandResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ProductLogMessage.FailedToUpdate(request.Id,ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("UpdateError", ex.Message) }, true);
                return ApiResponse<UpdateProductCommandResponse>.Fail(error);
            }
            


        }
    }
}
