using CompanyOrderManagement.Application.ResponseModels;
using MediatR;
namespace CompanyOrderManagement.Application.Features.Companies.Commands.Delete
{
    public class DeleteCompanyCommandRequest : IRequest<NoContentResponse>
    {
        public Guid Id { get; set; }

        public DeleteCompanyCommandRequest(Guid id) => Id = id;
    }
}
