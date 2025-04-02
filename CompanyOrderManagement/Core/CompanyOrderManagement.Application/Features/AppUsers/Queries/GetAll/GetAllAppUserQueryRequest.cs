using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.AppUsers.Queries.GetAll
{
    public class GetAllAppUserQueryRequest : IRequest<ApiResponse<List<GetAllAppUserQueryResponse>>>
    {
    }
}
