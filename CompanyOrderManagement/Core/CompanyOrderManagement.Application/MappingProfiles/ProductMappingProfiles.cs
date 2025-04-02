using AutoMapper;
using CompanyOrderManagement.Application.Features.Products.Commands.Create;
using CompanyOrderManagement.Application.Features.Products.Commands.Delete;
using CompanyOrderManagement.Application.Features.Products.Commands.Update;
using CompanyOrderManagement.Application.Features.Products.Queries.GetAll;
using CompanyOrderManagement.Application.Features.Products.Queries.GetById;
using CompanyOrderManagement.Domain.Entities;

namespace CompanyOrderManagement.Application.MappingProfiles
{
    public class ProductMappingProfiles : Profile
    {
        public ProductMappingProfiles()
        {
            CreateMap<CreateProductCommandRequest, Product>();
            CreateMap<DeleteProductCommandRequest, Product>();
            CreateMap<UpdateProductCommandRequest, Product>();
            CreateMap<GetAllProductQueryRequest, Product>();
            CreateMap<GetByIdProductQueryRequest, Product>();

            CreateMap<Product, CreateProductCommandResponse>();
            CreateMap<Product, DeleteProductCommandResponse>();
            CreateMap<Product, UpdateProductCommandResponse>();
            CreateMap<Product, GetAllProductQueryResponse>();
            CreateMap<Product, GetByIdProductQueryResponse>();
        }

    }

}
