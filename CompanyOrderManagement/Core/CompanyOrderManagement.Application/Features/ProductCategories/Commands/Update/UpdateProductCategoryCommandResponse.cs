

namespace CompanyOrderManagement.Application.Features.ProductCategories.Commands.Update
{
    public class UpdateProductCategoryCommandResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get;  set; }

    }
}
