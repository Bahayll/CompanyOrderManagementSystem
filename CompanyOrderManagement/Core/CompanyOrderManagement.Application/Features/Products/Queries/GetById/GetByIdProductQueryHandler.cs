using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Products;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.ProductRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Products;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Products.Queries.GetById
{
    public class GetByIdProductQueryHandler : IRequestHandler<GetByIdProductQueryRequest, ApiResponse<GetByIdProductQueryResponse>>
    {
        private readonly IProductUnitOfWork _productUnitOfWork;
        private readonly IProductBusinessRules _productBusinessRules;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public GetByIdProductQueryHandler(IProductUnitOfWork productUnitOfWork, IProductBusinessRules productBusinessRules, IMapper mapper, ILoggerService logger)
        {
            _productUnitOfWork = productUnitOfWork;
            _logger = logger;
            _productBusinessRules = productBusinessRules;
            _mapper = mapper;
        }

        public async Task<ApiResponse<GetByIdProductQueryResponse>> Handle(GetByIdProductQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _productBusinessRules.EnsureProductIdExists(request.Id);

                var product = await _productUnitOfWork.GetReadRepository.GetByIdAsync(request.Id);
                var response = _mapper.Map<GetByIdProductQueryResponse>(product);
                _logger.Info(ProductLogMessage.SuccessfullyFetchedById(request.Id));
                return ApiResponse<GetByIdProductQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.Error(ProductLogMessage.FailedToFetchById(request.Id,ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("FetchByIdError", ex.Message) }, true);
                return ApiResponse<GetByIdProductQueryResponse>.Fail(error);
            }
          

        }
    }
}
