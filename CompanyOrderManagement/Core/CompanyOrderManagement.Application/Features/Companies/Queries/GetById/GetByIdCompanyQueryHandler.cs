using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Companies;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Repositories.CompanyRepository;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Companies;
using CompanyOrderManagement.Application.Services.Cache;
using CompanyOrderManagement.Domain.Entities;
using MediatR;

namespace CompanyOrderManagement.Application.Features.Companies.Queries.GetById
{
    public class GetByIdCompanyQueryHandler : IRequestHandler<GetByIdCompanyQueryRequest, ApiResponse<GetByIdCompanyQueryResponse>>
    {
        private readonly ICompanyUnitOfWork _companyUnitOfWork;
        private readonly ICompanyBusinessRules _companyBusinessRules;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        private readonly ICacheService _cacheService;

        public GetByIdCompanyQueryHandler(ICompanyUnitOfWork companyUnitOfWork, ICompanyBusinessRules companyBusinessRules, IMapper mapper, ILoggerService logger, ICacheService cacheService)
        {
            _companyUnitOfWork = companyUnitOfWork;
            _companyBusinessRules = companyBusinessRules;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<GetByIdCompanyQueryResponse>> Handle(GetByIdCompanyQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _companyBusinessRules.EnsureCompanyIdExists(request.Id);

                var cacheKey = CacheKeys.CompanyById(request.Id);
                var cachedCompany = _cacheService.Get<Company>(cacheKey);
                if (cachedCompany != null)
                {
                    _logger.Info(CompanyLogMessage.CacheSuccessfullyFetchedById(request.Id));
                    var responseFromCache = _mapper.Map<GetByIdCompanyQueryResponse>(cachedCompany);
                    return ApiResponse<GetByIdCompanyQueryResponse>.Success(responseFromCache);
                }

                var company = await _companyUnitOfWork.GetReadRepository.GetByIdAsync(request.Id);
                _cacheService.Set(cacheKey, company, TimeSpan.FromHours(1));

                _logger.Info(CompanyLogMessage.SuccessfullyFetchedById(request.Id));

                var response = _mapper.Map<GetByIdCompanyQueryResponse>(company);
                return ApiResponse<GetByIdCompanyQueryResponse>.Success(response);

            }
            catch (Exception ex)
            {
                _logger.Error(CompanyLogMessage.FailedToFetchById(request.Id,ex.Message));

                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("FetchByIdError", ex.Message) }, true);
                return ApiResponse<GetByIdCompanyQueryResponse>.Fail(error);
            }

        }
    }
}
