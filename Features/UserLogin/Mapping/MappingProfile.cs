using AutoMapper;
using Domains;
using Features.UserLogin.Requests;

namespace Features.UserLogin.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<User, InformationDetailRequest>().ReverseMap();
        }
    }
}
