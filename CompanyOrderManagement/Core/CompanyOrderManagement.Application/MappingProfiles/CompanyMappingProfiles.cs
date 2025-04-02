using AutoMapper;
using CompanyOrderManagement.Application.Features.Companies.Commands.Create;
using CompanyOrderManagement.Application.Features.Companies.Commands.Delete;
using CompanyOrderManagement.Application.Features.Companies.Commands.Update;
using CompanyOrderManagement.Application.Features.Companies.Queries.GetAll;
using CompanyOrderManagement.Application.Features.Companies.Queries.GetById;
using CompanyOrderManagement.Domain.Entities;



namespace CompanyOrderManagement.Application.MappingProfiles
{
    public class CompanyMappingProfiles : Profile
    {
        public CompanyMappingProfiles()
        {
            
            CreateMap<CreateCompanyCommandRequest, Company>();
            CreateMap<DeleteCompanyCommandRequest, Company>();
            CreateMap<UpdateCompanyCommandRequest, Company>();
            CreateMap<GetAllCompanyQueryRequest, Company>();
            CreateMap<GetByIdCompanyQueryRequest, Company>();

            CreateMap<Company, CreateCompanyCommandResponse>();
            CreateMap<Company,DeleteCompanyCommandResponse>();
            CreateMap<Company,UpdateCompanyCommandResponse>();
            CreateMap<Company, GetAllCompanyQueryResponse>();    
            CreateMap<Company,GetByIdCompanyQueryResponse>();




        }
    }
}
