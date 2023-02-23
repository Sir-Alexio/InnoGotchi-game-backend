using AutoMapper;
using InnoGotchi_backend.Models;

namespace InnoGotchi_backend.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
