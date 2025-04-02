using CompanyOrderManagement.Application.ResponseModels;
using MediatR;

namespace CompanyOrderManagement.Application.Features.Products.Queries.GetById
{
    public class GetByIdProductQueryRequest : IRequest<ApiResponse<GetByIdProductQueryResponse>>
    {
        public Guid Id { get; set; }
        public GetByIdProductQueryRequest(Guid id) => Id = id;
    }
}
