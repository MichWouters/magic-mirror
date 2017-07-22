using System;

namespace MagicMirror.Business.Helpers
{
    public static class TemperatureHelper
    {
        public static double ConvertToCelsius(double degK)
        {
            return Math.Round((degK - 273.15) * 100) / 100;
        }

        public static double ConvertToCelsiusRounded(double degK)
        {
            return Math.Round(degK - 273.15);
        }

        public static double ConvertToFahrenheit(double degK)
        {
            return Math.Round(1.8 * (degK - 273) + 32);
        }

        public static double ConvertToFahrenheitRounded(double degK)
        {
            return Math.Round((1.8 * (degK - 273) + 32) * 100 / 100);
        }
    }
}
