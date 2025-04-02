using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Users;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Users;
using CompanyOrderManagement.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace CompanyOrderManagement.Application.Features.AppUsers.Queries.GetById
{
    public class GetByIdAppUserQueryHandler : IRequestHandler<GetByIdAppUserQueryRequest, ApiResponse<GetByIdAppUserQueryResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserBusinessRules _userBusinessRules;
        private readonly ILoggerService _logger;

        public GetByIdAppUserQueryHandler(UserManager<AppUser> userManager, IMapper mapper, ILoggerService logger, IUserBusinessRules userBusinessRules)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<ApiResponse<GetByIdAppUserQueryResponse>> Handle(GetByIdAppUserQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _userBusinessRules.EnsureUserIdExists(request.Id);
                var user = await _userManager.FindByIdAsync(request.Id.ToString());
                var response = _mapper.Map<GetByIdAppUserQueryResponse>(user);
                _logger.Info(UserLogMessage.SuccessfullyFetchedById(request.Id));
                return ApiResponse<GetByIdAppUserQueryResponse>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.Error(UserLogMessage.FailedToFetchById(request.Id, ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("FetchByIdError", ex.Message) }, true);
                return ApiResponse<GetByIdAppUserQueryResponse>.Fail(error);
            }
        
        }
    }
}
