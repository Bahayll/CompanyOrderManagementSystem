using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Products.Commands.Create
{
    public class CreateProductCommandRequest : IRequest<ApiResponse<CreateProductCommandResponse>>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
        public Guid CompanyId { get; set; }
        public Guid ProductCategoryId { get; set; }
    }
}
