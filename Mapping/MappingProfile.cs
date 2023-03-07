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
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(scr => scr.UserName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(scr => scr.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(scr => scr.LastName))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(scr => scr.Avatar))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(scr => scr.Email))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            //CreateMap<Farm, FarmDto>();
            //CreateMap<FarmDto, Farm>();
        }
    }
}
