using Xunit;

namespace Acme.Generic.Tests
{
    public class StringTests
    {
        [Fact]
        public void TimeNotationTests()
        {
            // Arrange
            int[] input = new int[] { 17, 35, 110, 120, 150, 210, 335, 447 };

            string[] expectedResults = new string[]
            {
                "0 hours and 17 minutes",
                "0 hours and 35 minutes",
                "1 hours and 50 minutes",
                "2 hours",
                "2 hours and 30 minutes",
                "3 hours and 30 minutes",
                "5 hours and 35 minutes",
                "7 hours and 27 minutes",
            };

            // Act
            for (int i = 0; i < input.Length; i++)
            {
                string result = input[i].GetTimeNotation();

                // Assert
                Assert.Equal(result, expectedResults[i]);
            }
        }
    }
}