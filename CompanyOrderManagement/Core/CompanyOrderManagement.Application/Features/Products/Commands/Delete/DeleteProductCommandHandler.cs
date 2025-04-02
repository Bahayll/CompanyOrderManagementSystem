using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Products;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Products;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Products.Commands.Delete
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, NoContentResponse>
    {
        private readonly IProductUnitOfWork _productUnitOfWork;
        private readonly IProductBusinessRules _productBusinessRules;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public DeleteProductCommandHandler(IProductUnitOfWork productUnitOfWork, IProductBusinessRules productBusinessRules, IMapper mapper, ILoggerService logger)
        {
            _productUnitOfWork = productUnitOfWork;
            _productBusinessRules = productBusinessRules;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<NoContentResponse> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            
            try
            {
                await _productBusinessRules.EnsureProductIdExists(request.Id);

                await _productUnitOfWork.GetWriteRepository.RemoveAsync(request.Id);
                await _productUnitOfWork.SaveAsync();
                _logger.Info(ProductLogMessage.SuccessfullyDeleted(request.Id));

                return NoContentResponse.Success();
            }
            catch (Exception ex)
            {
                _logger.Error(ProductLogMessage.FailedToDelete(request.Id,ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("DeleteError", ex.Message) }, true);
                return NoContentResponse.Fail(error);
            }
            
        }
    }
}
