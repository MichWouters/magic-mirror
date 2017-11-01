using Xunit;

namespace Acme.Generic.Tests
{
    public class UnitOfMeasureTests
    {
        [Fact]
        public void Miles_To_Kilometers_Correct()
        {
            // Arrange
            double[] inputs = { 1, 25, 37.33 };
            double[] expectedValues = { 1.61, 40.23, 60.08 };

            for (int i = 0; i < inputs.Length; i++)
            {
                // Act
                double result = DistanceHelper.MilesToKiloMeters(inputs[i]);

                // Assert
                Assert.Equal(result, expectedValues[i]);
            }
        }

        [Fact]
        public void Kilometers_To_Miles_Correct()
        {
            // Arrange
            double[] inputs = { 1, 25, 37.33 };
            double[] expectedValues = { 0.62, 15.53, 23.2 };

            for (int i = 0; i < inputs.Length; i++)
            {
                // Act
                double result = DistanceHelper.KiloMetersToMiles(inputs[i]);

                // Assert
                Assert.Equal(result, expectedValues[i]);
            }
        }
    }
}