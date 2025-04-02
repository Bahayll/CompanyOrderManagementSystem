using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Companies.Commands.Update
{
    public class UpdateCompanyCommandRequest : IRequest<ApiResponse<UpdateCompanyCommandResponse>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Email { get; set; }
    }
}
