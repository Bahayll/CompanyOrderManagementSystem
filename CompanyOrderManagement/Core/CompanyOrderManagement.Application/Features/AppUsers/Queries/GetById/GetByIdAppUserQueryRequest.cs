using CompanyOrderManagement.Application.ResponseModels;
using MediatR;

namespace CompanyOrderManagement.Application.Features.AppUsers.Queries.GetById
{
    public class GetByIdAppUserQueryRequest : IRequest<ApiResponse<GetByIdAppUserQueryResponse>>
    {
        public Guid Id { get; set; }

        public GetByIdAppUserQueryRequest(Guid id) => Id = id;
    }
}
