using CompanyOrderManagement.Application.ResponseModels;
using MediatR;


namespace CompanyOrderManagement.Application.Features.Orders.Queries.GetById
{
    public class GetByIdOrderQueryRequest : IRequest<ApiResponse<GetByIdOrderQueryResponse>>
    {
        public Guid Id { get; set; }

        public GetByIdOrderQueryRequest(Guid id)   => Id = id;
        
    }
}
