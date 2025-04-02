

namespace CompanyOrderManagement.Application.Features.Products.Commands.Delete
{
    public class DeleteProductCommandResponse
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
    }
}
