using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Products.Queries.GetAll
{
    public class GetAllProductQueryRequest : IRequest<ApiResponse<List<GetAllProductQueryResponse>>>
    {
    }
}
