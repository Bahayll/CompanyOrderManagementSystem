using CompanyOrderManagement.Application.ResponseModels;
using MediatR;

namespace CompanyOrderManagement.Application.Features.AppUsers.Commands.Create
{
    public class CreateAppUserCommandRequest :IRequest<ApiResponse<CreateAppUserCommandResponse>>
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }  
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

    }
}
