using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.DataAccess;
using System;
using System.Threading.Tasks;

namespace MagicMirror.ConsoleCore
{
    public class Program
    {
        private static IService<WeatherModel> _service;

        public static void Main(string[] args)
        {
            SearchCriteria criteria = GatherUserInformation();
            Console.WriteLine("Crunching the numbers...");

            Task.Run(() => GenerateInformation(criteria));
            Console.ReadLine();
        }

        private static SearchCriteria GatherUserInformation()
        {
            Console.WriteLine("Hello User!");
            Console.WriteLine("Please enter your name: ");
            string user = Console.ReadLine();

            Console.WriteLine("Please enter your home town: ");
            string homeAddress = Console.ReadLine();

            Console.WriteLine("Please enter your work address: ");
            string workAddress = Console.ReadLine();

            var criteria = new SearchCriteria
            {
                UserName = user,
                City = homeAddress,
                Destination = workAddress
            };

            return criteria;
        }

        private static async Task<WeatherModel> GenerateInformation(SearchCriteria criteria)
        {
            _service = new WeatherService();
            WeatherModel result = await _service.GetModelAsync(criteria);

            Console.WriteLine($"Welcome {criteria.UserName} The current top-side temperature is {result.TemperatureCelsius} degrees Celsius");
            return result;
        }
    }
}