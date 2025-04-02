using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Companies.Commands.Create
{
    public class CreateCompanyCommandRequest : IRequest<ApiResponse<CreateCompanyCommandResponse>>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Email {  get; set; }

    }
}
