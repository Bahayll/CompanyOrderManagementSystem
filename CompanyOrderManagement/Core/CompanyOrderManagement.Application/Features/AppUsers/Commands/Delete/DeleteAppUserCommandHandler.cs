using CompanyOrderManagement.Application.Logging.LogMessages.Users;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Users;
using CompanyOrderManagement.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CompanyOrderManagement.Application.Features.AppUsers.Commands.Delete
{
    public class DeleteAppUserCommandHandler : IRequestHandler<DeleteAppUserCommandRequest, NoContentResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILoggerService _logger;
        private readonly IUserBusinessRules _userBusinessRules;


        public DeleteAppUserCommandHandler(UserManager<AppUser> userManager, ILoggerService loggerService, IUserBusinessRules userBusinessRules)
        {
            _userManager = userManager;
            _logger = loggerService;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<NoContentResponse> Handle(DeleteAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _userBusinessRules.EnsureUserIdExists(request.Id);

                var user = await _userManager.FindByIdAsync(request.Id.ToString());
                var result = await _userManager.DeleteAsync(user);
                _logger.Info(UserLogMessage.SuccessfullyDeleted(request.Id));

                return NoContentResponse.Success();
            }
            catch (Exception ex)
            {
                _logger.Error(UserLogMessage.FailedToDelete(request.Id, ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("DeleteError", ex.Message) }, true);
                return NoContentResponse.Fail(error);
            }

        }
    }
}
