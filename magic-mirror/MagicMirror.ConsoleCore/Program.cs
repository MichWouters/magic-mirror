using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.DataAccess;
using System;
using System.Threading.Tasks;

namespace MagicMirror.ConsoleCore
{
    public class Program
    {
        private static IService<WeatherModel> _weatherService;
        private static IService<TrafficModel> _trafficService;

        public static void Main(string[] args)
        {
            SearchCriteria criteria = GatherUserInformation();
            Console.WriteLine("Crunching the numbers...");
            Console.WriteLine();

            MagicMirrorDto dto = Task.Run(() => GenerateDto(criteria)).Result;
            OutputData(dto);
            Console.ReadLine();
        }

        private static SearchCriteria GatherUserInformation()
        {
            Console.WriteLine("Hello User!");
            Console.WriteLine("Please enter your name: ");
            string user = Console.ReadLine();

            Console.WriteLine("Please enter your street and house number: ");
            string address = Console.ReadLine();

            Console.WriteLine("Please enter your home town: ");
            string city = Console.ReadLine();

            Console.WriteLine("Please enter your work address: ");
            string workAddress = Console.ReadLine();

            var criteria = new SearchCriteria
            {
                UserName = user,
                Start = $"{address}, {city}",
                City = city,
                Destination = workAddress,
            };

            return criteria;
        }

        private static async Task<MagicMirrorDto> GenerateDto(SearchCriteria criteria)
        {
            try
            {
                _weatherService = new WeatherService();
                _trafficService = new TrafficService();
                WeatherModel weatherModel = await _weatherService.GetModelAsync(criteria);
                TrafficModel trafficModel = await _trafficService.GetModelAsync(criteria);

                var dto = new MagicMirrorDto
                {
                    UserName = criteria.UserName,
                    DegreesCelsius = weatherModel.TemperatureCelsius,
                    TravelTime = trafficModel.MinutesText,
                    TrafficDensity = trafficModel.TrafficDensity,
                    WeatherType = weatherModel.WeatherType
                };

                return dto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void OutputData(MagicMirrorDto dto)
        {
            Console.WriteLine($"Hello {dto.UserName}");
            Console.WriteLine($"Today is {DateTime.Now:D}. The current time is: {DateTime.Now:t}");
            Console.WriteLine($"The current top-side temperature is {dto.DegreesCelsius} degrees Celsius with {dto.WeatherType}");
            Console.WriteLine($"Your travel time is {dto.TravelTime} with {dto.TrafficDensity} traffic.");
        }
    }
}