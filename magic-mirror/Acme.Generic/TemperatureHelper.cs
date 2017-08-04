using System;

namespace Acme.Generic
{
    public static class TemperatureHelper
    {
        public static double KelvinToCelsius(double degK, int precision = 2)
        {
            double result = degK - 273.15;
            return Math.Round(result, precision);
        }

        public static double KelvinToFahrenheit(double degK, int precision = 2)
        {
            double result = 1.8 * (degK - 273) + 32;
            return Math.Round(result, precision);
        }

        public static double CelsiusToKelvin(double celsius, int precision = 2)
        {
            double result = celsius + 273.15;
            return Math.Round(result, precision);
        }

        public static double CelsiusToFahrenheit(double celsius, int precision = 2)
        {
            double result = (celsius * 1.8) + 32;
            return Math.Round(result, precision);
        }

        public static double FahrenheitToKelvin(double fahrenheit, int precision = 2)
        {
            double buffer = (double)(5m / 9m);
            double result = (buffer * (fahrenheit - 32)) + 273;
            return Math.Round(result, precision);
        }

        public static double FahrenheitToCelsius(double fahrenheit, int precision = 2)
        {
            double result = (fahrenheit - 32) / 1.8;
            return Math.Round(result, precision);
        }
    }
}