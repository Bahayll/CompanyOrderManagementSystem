
namespace CompanyOrderManagement.Application.Features.ProductCategories.Queries.GetById
{
    public class GetByIdProductCategoryQueryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
