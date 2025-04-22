using AutoMapper;
using Domains;
using Features.DailyJobs.DTOs;

namespace Features.DailyJobs.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<DailyPlan, DailyPlanDTO>().ReverseMap();
        }
    }
}
