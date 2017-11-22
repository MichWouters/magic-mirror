using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace MagicMirror.UniversalApp.Services
{
    public class LocationService : ILocationService
    {
        public LocationService()
        {
        }

        public async Task<Geoposition> GetLocationAsync()
        {
            var access = await RequestAccessAsync();
            var pos = await GetCoordinates();

            return pos;
        }

        private async Task<GeolocationAccessStatus> RequestAccessAsync()
        {
            GeolocationAccessStatus accessStatus = await Geolocator.RequestAccessAsync();

            switch (accessStatus)
            {
                case GeolocationAccessStatus.Denied:
                    throw new UnauthorizedAccessException("Location Service is not available at this time. Location service for this device may be disabled or denied");
                case GeolocationAccessStatus.Unspecified:
                    throw new ArgumentException("Location Service is not available at this time. Unspecified error");
            }
            return accessStatus;
        }

        private async Task<Geoposition> GetCoordinates()
        {
            Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 25 };
            Geoposition position = await geolocator.GetGeopositionAsync();

            geolocator.PositionChanged += OnPositionChanged;

            return position;
        }

        private void OnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            var latitude = args.Position.Coordinate.Point.Position.Latitude;
            var longitude = args.Position.Coordinate.Point.Position.Longitude;
        }
    }
}