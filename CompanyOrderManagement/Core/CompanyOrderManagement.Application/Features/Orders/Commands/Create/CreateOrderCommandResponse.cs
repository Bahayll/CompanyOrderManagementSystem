
namespace CompanyOrderManagement.Application.Features.Orders.Commands.Create
{
    public class CreateOrderCommandResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ProductCount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Address { get; set; }
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public ICollection<Guid>? ProductsId { get; set; }
      

    }
}
