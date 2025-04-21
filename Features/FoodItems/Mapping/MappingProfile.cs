using AutoMapper;
using Domains;
using Features.FoodItems.Requests;

namespace Features.FoodItems.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<FoodItemRequest, ItemBreakfast>()
                .ForMember(dest => dest.Carb,
                            otp => otp.MapFrom(src => src.Carbs))
                .ForMember(dest => dest.Amount,
                            otp => otp.MapFrom(src => src.Grams))
                .ReverseMap();
            
            CreateMap<FoodItemRequest, ItemLunch>()
                .ForMember(dest => dest.Carb,
                            otp => otp.MapFrom(src => src.Carbs))
                .ForMember(dest => dest.Amount,
                            otp => otp.MapFrom(src => src.Grams))
                .ReverseMap();
            
            CreateMap<FoodItemRequest, ItemDinner>()
                .ForMember(dest => dest.Carb,
                            otp => otp.MapFrom(src => src.Carbs))
                .ForMember(dest => dest.Amount,
                            otp => otp.MapFrom(src => src.Grams))
                .ReverseMap();
        }
    }
}
