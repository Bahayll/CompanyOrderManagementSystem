using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Companies.Queries.GetAll
{
    public class GetAllCompanyQueryRequest : IRequest<ApiResponse<List<GetAllCompanyQueryResponse>>>
    {


    }
}
