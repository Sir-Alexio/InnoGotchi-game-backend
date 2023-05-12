using AutoMapper;
using InnoGotchi_backend.Models;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.DTOs;

namespace InnoGotchi_backend.Extensions.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(scr => scr.UserName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(scr => scr.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(scr => scr.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(scr => scr.Email))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(scr => scr.Avatar));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(scr => scr.UserName))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(scr => scr.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(scr => scr.LastName))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(scr => scr.Avatar))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(scr => scr.Email))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<PetDto, Pet>();
                
            CreateMap<Farm, FarmDto>()
                .ForMember(dest => dest.FarmName, opt => opt.MapFrom(scr => scr.FarmName))
                .ForMember(dest => dest.DeadPetsCount, opt => opt.MapFrom(scr => scr.DeadPetsCount))
                .ForMember(dest => dest.AlivePetsCount, opt => opt.MapFrom(scr => scr.AlivePetsCount));

            CreateMap<Pet, PetDto>()
           .ForMember(dest => dest.PetName, opt => opt.MapFrom(src => src.PetName))
           .ForMember(dest => dest.AgeDate, opt => opt.MapFrom(src => src.AgeDate))
           .ForMember(dest => dest.LastHungerLevel, opt => opt.MapFrom(src => src.LastHungerLevel))
           .ForMember(dest => dest.LastThirstyLevel, opt => opt.MapFrom(src => src.LastThirstyLevel))
           .ForMember(dest => dest.HappyDaysCount, opt => opt.MapFrom(src => src.HappyDaysCount));

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

        }
    }
}
