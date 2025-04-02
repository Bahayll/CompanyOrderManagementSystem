using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.ProductCategories.Queries.GetById
{
    public class GetByIdProductCategoryQueryRequest : IRequest<ApiResponse<GetByIdProductCategoryQueryResponse>>
    {
        public Guid Id { get; set; }

        public GetByIdProductCategoryQueryRequest(Guid id) => Id = id;
     
    }
}
