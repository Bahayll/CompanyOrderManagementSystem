

namespace CompanyOrderManagement.Application.Features.ProductCategories.Commands.Create
{
    public class CreateProductCategoryCommandResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
     
    }
}
