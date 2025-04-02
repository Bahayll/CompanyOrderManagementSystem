using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Orders;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.OrderRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Orders;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Orders.Commands.Delete
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommandRequest, NoContentResponse>
    {
        private readonly IOrderUnitOfWork _orderUnitOfWork;
        private readonly IOrderBusinessRules _orderBusinessRules;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public DeleteOrderCommandHandler(IOrderUnitOfWork orderUnitOfWork, IOrderBusinessRules orderBusinessRules, IMapper mapper, ILoggerService logger)
        {
            _orderUnitOfWork = orderUnitOfWork;
            _orderBusinessRules = orderBusinessRules;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<NoContentResponse> Handle(DeleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _orderBusinessRules.EnsureOrderIdExists(request.Id);

                await _orderUnitOfWork.GetWriteRepository.RemoveAsync(request.Id);
                await _orderUnitOfWork.SaveAsync();
                _logger.Info(OrderLogMessage.SuccessfullyDeleted(request.Id));

                return NoContentResponse.Success();
            }
            catch (Exception ex)
            {
                _logger.Error(OrderLogMessage.FailedToDelete(request.Id,ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("DeleteError", ex.Message) }, true);
                return NoContentResponse.Fail(error);
            }
          

        }
    }
}
