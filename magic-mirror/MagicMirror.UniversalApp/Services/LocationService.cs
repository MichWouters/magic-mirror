using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace MagicMirror.UniversalApp.Services
{
    public class LocationService : ILocationService
    {
        public async Task GetLocationAsync()
        {
            var access = await RequestAccessAsync();
            await GetCoordinates();
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

        private async Task GetCoordinates()
        {
            Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 25 };
            Geoposition pos = await geolocator.GetGeopositionAsync();
        }

        private async Task GetAddress(double latitude, double longitude)
        {
        }
    }
}