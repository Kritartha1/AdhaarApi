using Adhaar.API.Models.Domain;
using Adhaar.API.Models.DTO;
using AutoMapper;

namespace Adhaar.API.Mappings
{
    public class AutoMapperprofiles:Profile
    {
        public AutoMapperprofiles() {

            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<AddUserRequestDto, User>().ReverseMap();
            CreateMap<UpdateUserRequestDto, User>().ReverseMap();
        }
    }
}
