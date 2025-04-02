using AutoMapper;
using CompanyOrderManagement.Application.Features.Orders.Commands.Create;
using CompanyOrderManagement.Application.Features.Orders.Commands.Delete;
using CompanyOrderManagement.Application.Features.Orders.Queries.GetAll;
using CompanyOrderManagement.Application.Features.Orders.Queries.GetById;
using CompanyOrderManagement.Domain.Entities;

namespace CompanyOrderManagement.Application.MappingProfiles
{
    public class OrderMappingProfiles : Profile
    {
        public OrderMappingProfiles()
        {
            CreateMap<CreateOrderCommandRequest, Order>();
            CreateMap<DeleteOrderCommandRequest, Order>();
            CreateMap<GetAllOrderQueryRequest, Order>();
            CreateMap<GetByIdOrderQueryRequest, Order>();

            CreateMap<Order, CreateOrderCommandResponse>();
            CreateMap<Order, DeleteOrderCommandResponse>();
            CreateMap<Order, GetAllOrderQueryResponse>().ForMember(dest => dest.ProductsId, opt => opt.MapFrom(src => src.Products.Select(p => p.Id).ToList()));

            CreateMap<Order, GetByIdOrderQueryResponse>().ForMember(dest => dest.ProductsId, opt => opt.MapFrom(src => src.Products.Select(p => p.Id).ToList()));

        }
    }
}
