using Adhaar.API.Models.Domain;
using Adhaar.API.Models.DTO;
using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Adhaar.API.Mappings
{
    [ExcludeFromCodeCoverage]
    public class AutoMapperprofiles:Profile
    {
        public AutoMapperprofiles() {

            CreateMap<User, UserDto>().ReverseMap();
          //  CreateMap<AddUserRequestDto, User>().ReverseMap();
           // CreateMap<UpdateUserRequestDto, User>().ReverseMap();
            CreateMap<ImageAd,ImageDto>().ReverseMap(); 
            CreateMap<AddImageRequestDto, ImageAd>().ReverseMap();
           // CreateMap<UpdateImageRequestDto, ImageAd>().ReverseMap();
        }
    }
}
