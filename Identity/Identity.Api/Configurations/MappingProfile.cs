using AutoMapper;
using Identity.Api.Models;
using Identity.Entities.Concrete;
using ValueBlue.Core.Entities.Concrete;

namespace Identity.Api.Configurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequest, UserDocument>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.Trim().ToUpper()))
                .ForPath(dest => dest.Role.RoleName, opt => opt.MapFrom(src => src.Role.ToUpper()));
        }
    }
}
