using Acme.Generic.Helpers;
using MagicMirror.Business.Enums;
using System;

namespace MagicMirror.Business.Models
{
    public class TrafficModel : Model
    {
        public string Origin { get; set; }

        public string Destination { get; set; }

        public double Distance { get; set; }

        public int Duration { get; set; }

        public DateTime TimeOfArrival { get; set; }

        public DistanceUom DistanceUom { get; set; }

        public override void ConvertValues()
        {
            Distance = ConvertDistance(Distance, DistanceUom);
            TimeOfArrival = CalculateTimeOfArrival(Duration);
        }

        private DateTime CalculateTimeOfArrival(int duration)
        {
            return DateTime.Now.AddMinutes(duration);
        }

        private double ConvertDistance(double distance, DistanceUom distanceUom)
        {
            double convertedDistance;

            switch (distanceUom)
            {
                case DistanceUom.Imperial:
                    convertedDistance = DistanceHelper.KiloMetersToMiles(Distance);
                    break;

                case DistanceUom.Metric:
                    convertedDistance = DistanceHelper.MilesToKiloMeters(Distance);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(DistanceUom), DistanceUom, null);
            }
            return convertedDistance;
        }
    }
}