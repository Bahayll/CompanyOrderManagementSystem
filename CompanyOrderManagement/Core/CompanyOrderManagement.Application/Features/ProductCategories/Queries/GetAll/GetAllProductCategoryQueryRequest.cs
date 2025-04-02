using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.ProductCategories.Queries.GetAll
{
    public class GetAllProductCategoryQueryRequest : IRequest<ApiResponse<List<GetAllProductCategoryQueryResponse>>>
    {
    }
}
