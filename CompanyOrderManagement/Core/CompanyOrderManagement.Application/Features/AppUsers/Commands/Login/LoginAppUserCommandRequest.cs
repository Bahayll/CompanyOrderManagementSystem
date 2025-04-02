using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.AppUsers.Commands.Login
{
    public class LoginAppUserCommandRequest : IRequest<ApiResponse<LoginAppUserCommandResponse>>
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
