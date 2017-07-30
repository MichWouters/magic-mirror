using MagicMirror.Business.Helpers;
using Xunit;

namespace MagicMirror.Tests.Helpers
{
    public class TemperatureTests
    {
        [Fact]
        public void Kelvin_To_Celsius_Correct()
        {
            // Arrange
            double kelvin = 290;

            // Act
            double result = TemperatureHelper.KelvinToCelsius(kelvin);
            double resultRounded = TemperatureHelper.KelvinToCelsius(kelvin, 0);

            // Assert
            Assert.Equal(16.85, result);
            Assert.Equal(17, resultRounded);
        }

        [Fact]
        public void Kelvin_To_Fahrenheit_Correct()
        {
            // Arrange
            double kelvin = 290;

            // Act
            double result = TemperatureHelper.KelvinToFahrenheit(kelvin);
            double resultRounded = TemperatureHelper.KelvinToFahrenheit(kelvin, 0);

            // Assert
            Assert.Equal(62.6, result);
            Assert.Equal(63, resultRounded);
        }

        [Fact]
        public void Celsius_To_Kelvin_Correct()
        {
            // Arrange
            double celsius = 20;

            // Act
            double result = TemperatureHelper.CelsiusToKelvin(celsius);
            double resultRounded = TemperatureHelper.CelsiusToKelvin(celsius, 0);

            // Assert
            Assert.Equal(293.15, result);
            Assert.Equal(293, resultRounded);
        }

        [Fact]
        public void Celsius_To_Fahrenheit_Correct()
        {
            // Arrange
            double celsius = 18;

            // Act
            double result = TemperatureHelper.CelsiusToFahrenheit(celsius);
            double resultRounded = TemperatureHelper.CelsiusToFahrenheit(celsius, 0);

            // Assert
            Assert.Equal(64.4, result);
            Assert.Equal(64, resultRounded);
        }

        [Fact]
        public void Fahrenheit_To_Kelvin_Correct()
        {
            // Arrange
            double fahrenheit = 100;

            // Act
            double result = TemperatureHelper.FahrenheitToKelvin(fahrenheit);
            double resultRounded = TemperatureHelper.FahrenheitToKelvin(fahrenheit, 0);

            // Assert
            Assert.Equal(310.78, result);
            Assert.Equal(311, resultRounded);
        }

        [Fact]
        public void Fahrenheit_To_Celsius_Correct()
        {
            // Arrange
            double fahrenheit = 100;

            // Act
            double result = TemperatureHelper.FahrenheitToCelsius(fahrenheit);
            double resultRounded = TemperatureHelper.FahrenheitToCelsius(fahrenheit, 0);

            // Assert
            Assert.Equal(37.78, result);
            Assert.Equal(38, resultRounded);
        }
    }
}