
namespace CompanyOrderManagement.Application.Features.Products.Queries.GetById
{
    public class GetByIdProductQueryResponse
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid CompanyId { get; set; }
        public Guid ProductCategoryId { get; set; }
    }
}
