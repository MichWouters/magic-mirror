using AutoMapper;
using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities.Traffic;
using MagicMirror.DataAccess.Entities.Weather;


namespace MagicMirror.Business.Configuration
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<WeatherEntity, WeatherModel>()
                .ForMember(x => x.Location, y => y.MapFrom(z => z.Name))
                .ForMember(x => x.Sunrise, y => y.MapFrom(z => z.Sys.Sunrise))
                .ForMember(x => x.Sunset, y => y.MapFrom(z => z.Sys.Sunset))
                .ForMember(x => x.Temperature, y => y.MapFrom(z => z.Main.Temp))
                .ForMember(x => x.WeatherType, y => y.MapFrom(z => z.Weather[0].Main));

            CreateMap<TrafficEntity, TrafficModel>()
                .ForMember(x => x.Destination, y => y.MapFrom(z => z.Destination_addresses[0]))
                .ForMember(x => x.Origin, y => y.MapFrom(z => z.Origin_addresses[0]))
                .ForMember(x => x.Distance, y => y.MapFrom(z => z.Rows[0].Elements[0].Distance.Value))
                .ForMember(x => x.Duration, y => y.MapFrom(z => z.Rows[0].Elements[0].Duration.Value));

            //CreateMap<AddressEntity, AddressModel>()
            //    .ForMember(dest => dest.HouseNumber, source => source.MapFrom(src => src.Results[0].address_components[0].long_name))
            //    .ForMember(dest => dest.Street, source => source.MapFrom(src => src.Results[0].address_components[1].long_name))
            //    .ForMember(dest => dest.City, source => source.MapFrom(src => src.Results[0].address_components[2].long_name))
            //    .ForMember(dest => dest.Country, source => source.MapFrom(src => src.Results[0].address_components[5].long_name))
            //    .ForMember(dest => dest.PostalCode, source => source.MapFrom(src => src.Results[0].address_components[6].long_name))
            //    .ReverseMap();

            //CreateMap<UserEntity, UserProfileModel>()
            //    .ForMember(dest => dest.FaceIds, source => source.MapFrom(src => src.Faces.Select(f => f.Id).ToArray()));

            //CreateMap<UserProfileModel, UserEntity>()
            //    .ForMember(dest => dest.Faces, source => source.MapFrom(src => src.FaceIds.Select(fId => new UserFace { Id = fId })));

            //CreateMap<UserAddress, UserAddressModel>()
            //    .ReverseMap();

            //CreateMap<RSSItem, RSSEntityItem>().ReverseMap();
            //CreateMap<RSSEntity, RSSModel>().ReverseMap();
        }
    }
}