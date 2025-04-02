using CompanyOrderManagement.Application.ResponseModels;
using MediatR;

namespace CompanyOrderManagement.Application.Features.ProductCategories.Commands.Update
{
    public class UpdateProductCategoryCommandRequest : IRequest<ApiResponse<UpdateProductCategoryCommandResponse>>
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
