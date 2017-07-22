using AutoMapper;
using MagicMirror.Business.Models.Traffic;
using MagicMirror.Business.Models.Weather;
using MagicMirror.DataAccess.Entities.Weather;
using MagicMirror.Entities.Traffic;

namespace MagicMirror.Business.Configuration
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<WeatherEntity, WeatherModel>()
                    .ForMember(dest => dest.SunRise, source => source.MapFrom(src => src.Sys.Sunrise))
                    .ForMember(dest => dest.SunSet, source => source.MapFrom(src => src.Sys.Sunset))
                    .ForMember(dest => dest.WeatherType, source => source.MapFrom(src => src.Weather[0].Main))
                    .ForMember(dest => dest.Icon, source => source.MapFrom(src => src.Weather[0].Icon))
                    .ForMember(dest => dest.Description, source => source.MapFrom(src => src.Weather[0].Description))
                .ReverseMap();

            CreateMap<TrafficEntity, TrafficModel>()
                .ReverseMap();
        }
    }
}
