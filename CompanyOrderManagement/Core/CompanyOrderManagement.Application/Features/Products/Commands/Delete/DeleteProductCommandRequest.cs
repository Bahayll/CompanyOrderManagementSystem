using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Products.Commands.Delete
{
    public class DeleteProductCommandRequest : IRequest<NoContentResponse>
    {
        public Guid Id { get; set; }
        public DeleteProductCommandRequest(Guid id) => Id = id; 
    }
}
