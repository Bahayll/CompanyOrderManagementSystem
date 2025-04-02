namespace CompanyOrderManagement.Application.Features.AppUsers.Commands.Create
{
    public class CreateAppUserCommandResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
