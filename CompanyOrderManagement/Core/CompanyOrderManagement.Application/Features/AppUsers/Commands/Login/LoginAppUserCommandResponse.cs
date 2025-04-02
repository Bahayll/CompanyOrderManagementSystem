using CompanyOrderManagement.Application.DTOs;
namespace CompanyOrderManagement.Application.Features.AppUsers.Commands.Login
{
    public class LoginAppUserCommandResponse
    {
        public string UsernameOrEmail { get; set; }
        public Token Token { get; set; }
    }

}
