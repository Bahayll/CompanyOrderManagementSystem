using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.AppUsers.Commands.Delete
{
    public class DeleteAppUserCommandRequest  : IRequest<NoContentResponse>
    {
        public Guid Id { get; set; }

        public DeleteAppUserCommandRequest(Guid id) => Id = id;
    }
}
