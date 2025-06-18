using AutoMapper;
using Domains;
using Features.UserFeatures.DTOs;
using Features.UserFeatures.Requests.Register;

namespace Features.UserFeatures.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User,RegisterRequest>().ReverseMap();
            CreateMap<User,NewUserResponseDTO>().ReverseMap();
        }
    }
}
