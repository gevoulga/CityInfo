using AutoMapper;

namespace CityInfo.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>()
                // .ForMember(dest => dest.Name,
                //     expression => expression.MapFrom(src=>src.Name.ToUpper()))
                .ReverseMap();
        }
    }
}