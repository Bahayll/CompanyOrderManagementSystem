
namespace CompanyOrderManagement.Application.Features.Companies.Commands.Create
{
    public class CreateCompanyCommandResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Email { get; set; }
    }
}
