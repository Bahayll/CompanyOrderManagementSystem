using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Orders.Commands.Create
{
    public class CreateOrderCommandRequest : IRequest<ApiResponse<CreateOrderCommandResponse>>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public int ProductCount { get; set; }
        public decimal UnitPrice { get; set; }
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public ICollection<Guid>? ProductsId { get; set; }
        
    }
}
