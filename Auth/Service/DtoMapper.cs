using AutoMapper;
using Core.Models.Dto;
using Core.Models.Entities;

namespace Service
{
    class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<Core.Models.Dto.Product, Core.Models.Entities.Product>().ReverseMap();
            CreateMap<User, AppUser>().ReverseMap();
        }
    }
}