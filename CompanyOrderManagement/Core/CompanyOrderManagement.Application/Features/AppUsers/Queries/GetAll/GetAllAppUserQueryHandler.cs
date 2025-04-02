using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Users;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace CompanyOrderManagement.Application.Features.AppUsers.Queries.GetAll
{
    public class GetAllAppUserQueryHandler : IRequestHandler<GetAllAppUserQueryRequest,ApiResponse< List<GetAllAppUserQueryResponse>>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public GetAllAppUserQueryHandler(UserManager<AppUser> userManager, AutoMapper.IMapper mapper, ILoggerService logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<List<GetAllAppUserQueryResponse>>> Handle(GetAllAppUserQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var users = _userManager.Users.ToList();
                var responses = _mapper.Map<List<GetAllAppUserQueryResponse>>(users);
                _logger.Info(UserLogMessage.SuccessfullyFetchedAll(responses.Count));
                return ApiResponse<List<GetAllAppUserQueryResponse>>.Success(responses);
            }
            catch (Exception ex)
            {
                _logger.Error(UserLogMessage.FailedToFetchAll(ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("FetchAllError", ex.Message) }, true);
                return ApiResponse<List<GetAllAppUserQueryResponse>>.Fail(error);
            }
        }
    }
}
