using AutoMapper;
using CompanyOrderManagement.Application.Logging.LogMessages.Users;
using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Users;
using CompanyOrderManagement.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CompanyOrderManagement.Application.Features.AppUsers.Commands.Update
{
    public class UpdateAppUserCommandHandler : IRequestHandler<UpdateAppUserCommandRequest,ApiResponse< UpdateAppUserCommandResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUserBusinessRules _userBusinessRules;
        private readonly ILoggerService _logger;

        public UpdateAppUserCommandHandler(UserManager<AppUser> userManager, IMapper mapper, IUserBusinessRules userBusinessRules, ILoggerService logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
            _logger = logger;
        }

        public async Task<ApiResponse<UpdateAppUserCommandResponse>> Handle(UpdateAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _userBusinessRules.EnsureUserIdExists(Guid.Parse(request.Id));
                await _userBusinessRules.EnsureUserDetailsAreUpdated(request);

                var user = await _userManager.FindByIdAsync(request.Id);
                _mapper.Map(request, user);

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    var error = new ErrorDTO(updateResult.Errors.Select(e => new ValidationError(e.Code, e.Description)).ToList(), true);
                    return ApiResponse<UpdateAppUserCommandResponse>.Fail(error);
                }

                if (!string.IsNullOrEmpty(request.Password))
                {
                    _userBusinessRules.EnsurePasswordsMatch(request.Password, request.ConfirmPassword);

                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, resetToken, request.Password);

                    if (!result.Succeeded)
                    {
                        var error = new ErrorDTO(result.Errors.Select(e => new ValidationError(e.Code, e.Description)).ToList(), true);
                        return ApiResponse<UpdateAppUserCommandResponse>.Fail(error);
                    }
                }

                var response = _mapper.Map<UpdateAppUserCommandResponse>(user);
                return ApiResponse<UpdateAppUserCommandResponse>.Success(response);

            }
            catch (Exception ex)
            {
                _logger.Error(UserLogMessage.FailedToUpdate(Guid.Parse(request.Id), ex.Message));
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("UpdateError", ex.Message) }, true);
                return ApiResponse<UpdateAppUserCommandResponse>.Fail(error);
            }
           
        }
    }

}
