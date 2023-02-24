using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;

namespace InnoGotchi_backend.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Farm, FarmDto>();
            CreateMap<FarmDto, Farm>();
        }
    }
}
