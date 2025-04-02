using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Companies.Queries.GetById
{
    public class GetByIdCompanyQueryRequest : IRequest<ApiResponse<GetByIdCompanyQueryResponse>>
    {
        public Guid Id { get; set; }

        public GetByIdCompanyQueryRequest(Guid id) => Id = id;

    }
}
