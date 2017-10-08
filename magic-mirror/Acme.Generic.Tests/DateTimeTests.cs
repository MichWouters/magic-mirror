using Acme.Generic.Extensions;
using System;
using Xunit;

namespace Acme.Generic.Tests
{
    public class DateTimeTests
    {
        [Fact]
        public void Int_To_DateTime_Correct()
        {
            // Arrange
            int sunrise = 1501302043;
            int sunset = 1501357935;

            // Act
            DateTime sunriseDate =sunrise.ConvertFromUnixTimestamp();
            DateTime sunsetDate = sunset.ConvertFromUnixTimestamp();

            // Assert
            DateTime expectedSunRise = new DateTime(2017, 7, 29, 4, 20, 43);
            DateTime expectedSunset = new DateTime(2017, 7, 29, 19, 52, 15);

            Assert.Equal(expectedSunRise, sunriseDate);
            Assert.Equal(expectedSunset, sunsetDate);
        }

        [Fact]
        public void DateTime_To_Int_Correct()
        {
            // Arrange
            DateTime sunriseDate = new DateTime(2017, 7, 29, 4, 20, 43);
            DateTime sunsetDate = new DateTime(2017, 7, 29, 19, 52, 15);

            // Act
            var sunrise = sunriseDate.ConvertToUnixTimestamp();
            var sunset = sunsetDate.ConvertToUnixTimestamp();

            // Assert
            Assert.Equal(1501294843, sunrise);
            Assert.Equal(1501350735, sunset);
        }
    }
}