using AutoMapper;
using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Create;
using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Delete;
using CompanyOrderManagement.Application.Features.ProductCategories.Commands.Update;
using CompanyOrderManagement.Application.Features.ProductCategories.Queries.GetAll;
using CompanyOrderManagement.Application.Features.ProductCategories.Queries.GetById;
using CompanyOrderManagement.Domain.Entities;

namespace CompanyOrderManagement.Application.MappingProfiles
{
    public class ProductCategoryMappingProfiles : Profile
    {
        public ProductCategoryMappingProfiles()
        {
            CreateMap<CreateProductCategoryCommandRequest, ProductCategory>();
            CreateMap<DeleteProductCategoryCommandRequest, ProductCategory>();
            CreateMap<UpdateProductCategoryCommandRequest, ProductCategory>();
            CreateMap<GetAllProductCategoryQueryRequest, ProductCategory>();
            CreateMap<GetByIdProductCategoryQueryRequest, ProductCategory>();

            CreateMap<ProductCategory, CreateProductCategoryCommandResponse>();
            CreateMap<ProductCategory, DeleteProductCategoryCommandResponse>();
            CreateMap<ProductCategory, UpdateProductCategoryCommandResponse>();
            CreateMap<ProductCategory, GetAllProductCategoryQueryResponse>();
            CreateMap<ProductCategory, GetByIdProductCategoryQueryResponse>();
        }
    }
}
