using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Products;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Application.ResponseModels;
using MediatR;

namespace CompanyOrderManagement.Application.Features.Products.Queries.GetAll
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, ApiResponse<List<GetAllProductQueryResponse>>>
    {
        private readonly IProductUnitOfWork _productUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public GetAllProductQueryHandler(IProductUnitOfWork unitOfWork, IMapper mapper, ILoggerService logger)
        {
            _productUnitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<List<GetAllProductQueryResponse>>> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var products = _productUnitOfWork.GetReadRepository.GetAll().ToList();
                var response = _mapper.Map<List<GetAllProductQueryResponse>>(products);
                _logger.Info(ProductLogMessage.SuccessfullyFetchedAll(response.Count));
                return ApiResponse<List<GetAllProductQueryResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ProductLogMessage.FailedToFetchAll(ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("FetchAllError", ex.Message) }, true);
                return ApiResponse<List<GetAllProductQueryResponse>>.Fail(error);
            }

        }
    }
}
