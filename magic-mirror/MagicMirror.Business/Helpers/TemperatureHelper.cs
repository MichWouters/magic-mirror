using System;

namespace MagicMirror.Business.Helpers
{
    public static class TemperatureHelper
    {
        public static double ConvertToCelsius(int degK)
        {
            return Math.Round((degK - 273.15) * 100) / 100;
        }

        public static double ConvertToCelsiusRounded(double degK)
        {
            return Math.Round(degK - 273.15);
        }

        public static double ConvertToFahrenheit(int degK)
        {
            return Math.Round(1.8 * (degK - 273) + 32);
        }
    }
}
