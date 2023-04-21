using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.DTOs;

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

            CreateMap<PetDto, Pet>();

            CreateMap<Pet, PetDto>()
           .ForMember(dest => dest.PetName, opt => opt.MapFrom(src => src.PetName))
           .ForMember(dest => dest.AgeDate, opt => opt.MapFrom(src => src.AgeDate))
           .ForMember(dest => dest.LastHungerLevel, opt => opt.MapFrom(src => src.LastHungerLevel))
           .ForMember(dest => dest.LastThirstyLevel, opt => opt.MapFrom(src => src.LastThirstyLevel))
           .ForMember(dest => dest.HappyDaysCount, opt => opt.MapFrom(src => src.HappyDaysCount));

            
        }
    }
}
