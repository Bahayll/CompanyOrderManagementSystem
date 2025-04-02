using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Orders;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.OrderRepository;
using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Orders.Queries.GetAll
{
    public class GetAllOrderQueryHandler : IRequestHandler<GetAllOrderQueryRequest, ApiResponse<List<GetAllOrderQueryResponse>>>
    {
        private readonly IOrderUnitOfWork _orderUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public GetAllOrderQueryHandler(IOrderUnitOfWork unitOfWork, IMapper mapper, ILoggerService logger)
        {
            _orderUnitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<List<GetAllOrderQueryResponse>>> Handle(GetAllOrderQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {

                var orders =  _orderUnitOfWork.GetReadRepository.GetAll().ToList();
                //var orders = await _orderUnitOfWork.GetReadRepository.GetAll().Include(o => o.Products).ToListAsync(cancellationToken);

                var response = _mapper.Map<List<GetAllOrderQueryResponse>>(orders);
                _logger.Info(OrderLogMessage.SuccessfullyFetchedAll(response.Count));

                return ApiResponse<List<GetAllOrderQueryResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.Error(OrderLogMessage.FailedToFetchAll(ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("FetchAllError", ex.Message) }, true);
                return ApiResponse<List<GetAllOrderQueryResponse>>.Fail(error);
            }
        }
    }
}
