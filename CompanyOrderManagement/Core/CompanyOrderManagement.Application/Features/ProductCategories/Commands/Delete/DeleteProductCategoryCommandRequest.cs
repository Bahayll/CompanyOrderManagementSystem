using CompanyOrderManagement.Application.ResponseModels;
using MediatR;

namespace CompanyOrderManagement.Application.Features.ProductCategories.Commands.Delete
{
    public class DeleteProductCategoryCommandRequest : IRequest<NoContentResponse>
    {
        public Guid Id { get; set; }

        public DeleteProductCategoryCommandRequest(Guid id) =>  Id = id;
        
    }
}
