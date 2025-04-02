using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Orders.Queries.GetAll
{
    public class GetAllOrderQueryRequest : IRequest<ApiResponse<List<GetAllOrderQueryResponse>>>
    {
    }
}
