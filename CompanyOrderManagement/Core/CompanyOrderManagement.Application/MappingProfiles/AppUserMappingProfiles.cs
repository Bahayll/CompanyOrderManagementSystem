using AutoMapper;
using CompanyOrderManagement.Application.Features.AppUsers.Commands.Create;
using CompanyOrderManagement.Application.Features.AppUsers.Commands.Delete;
using CompanyOrderManagement.Application.Features.AppUsers.Commands.Login;
using CompanyOrderManagement.Application.Features.AppUsers.Commands.Update;
using CompanyOrderManagement.Application.Features.AppUsers.Queries.GetAll;
using CompanyOrderManagement.Application.Features.AppUsers.Queries.GetById;
using CompanyOrderManagement.Domain.Entities.Identity;

namespace CompanyOrderManagement.Application.MappingProfiles
{
    public class AppUserMappingProfiles : Profile
    {
        public AppUserMappingProfiles()
        {
            CreateMap<CreateAppUserCommandRequest, AppUser>();
            CreateMap<DeleteAppUserCommandRequest, AppUser>();
            CreateMap<UpdateAppUserCommandRequest, AppUser>();
            CreateMap<GetAllAppUserQueryRequest, AppUser>();
            CreateMap<GetByIdAppUserQueryRequest, AppUser>();
            CreateMap<LoginAppUserCommandRequest,AppUser>();

            CreateMap<AppUser, CreateAppUserCommandResponse>();
            CreateMap<AppUser, DeleteAppUserCommandResponse>();
            CreateMap<AppUser, UpdateAppUserCommandResponse>();
            CreateMap<AppUser, GetAllAppUserQueryResponse>();
            CreateMap<AppUser, GetByIdAppUserQueryResponse>();
            CreateMap<AppUser, LoginAppUserCommandResponse>();
        }
    }
}
