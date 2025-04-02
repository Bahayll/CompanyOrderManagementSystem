using CompanyOrderManagement.Domain.Enums;


namespace CompanyOrderManagement.Application.Features.Orders.Queries.GetAll
{
    public class GetAllOrderQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ProductCount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Address { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public ICollection<Guid>? ProductsId { get; set; }
    }
}
