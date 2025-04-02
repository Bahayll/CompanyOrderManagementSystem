using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.ProductCategories.Commands.Create
{
    public class CreateProductCategoryCommandRequest : IRequest<ApiResponse<CreateProductCategoryCommandResponse>>
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
