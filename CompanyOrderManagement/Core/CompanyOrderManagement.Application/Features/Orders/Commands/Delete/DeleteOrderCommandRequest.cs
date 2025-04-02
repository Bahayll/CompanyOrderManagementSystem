using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Orders.Commands.Delete
{
    public class DeleteOrderCommandRequest : IRequest<NoContentResponse>
    {
        public Guid Id { get; set; }

        public DeleteOrderCommandRequest(Guid id) => Id = id;
    }
}
