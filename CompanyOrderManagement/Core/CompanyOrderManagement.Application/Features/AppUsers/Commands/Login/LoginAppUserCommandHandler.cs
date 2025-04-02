using AutoMapper;
using CompanyOrderManagement.Application.Abstractions.Tokens;
using CompanyOrderManagement.Application.DTOs;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Application.Rules.Users;
using CompanyOrderManagement.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;


namespace CompanyOrderManagement.Application.Features.AppUsers.Commands.Login
{
    public class LoginAppUserCommandHandler : IRequestHandler<LoginAppUserCommandRequest,ApiResponse<LoginAppUserCommandResponse>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IUserBusinessRules _userBusinessRules;

        public LoginAppUserCommandHandler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ITokenHandler tokenHandler, IUserBusinessRules userBusinessRules, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<ApiResponse<LoginAppUserCommandResponse>> Handle(LoginAppUserCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _userBusinessRules.EnsureUserExistsByUsernameOrEmailAsync(request.UsernameOrEmail);

                var user = await _userManager.FindByNameAsync(request.UsernameOrEmail) ?? await _userManager.FindByEmailAsync(request.UsernameOrEmail);
                await _userBusinessRules.EnsurePasswordIsValidAsync(user, request.Password);

                Token token = _tokenHandler.CreateAccessToken(5);

                var response = new LoginAppUserCommandResponse { Token = token,UsernameOrEmail=request.UsernameOrEmail };

                return ApiResponse<LoginAppUserCommandResponse>.Success(response);
            }
            catch (Exception ex)
            {
                var error = new ErrorDTO(new List<ValidationError> { new ValidationError("LoignError", ex.Message) }, true);
                return ApiResponse<LoginAppUserCommandResponse>.Fail(error);
            }



           

        }
    }
}
