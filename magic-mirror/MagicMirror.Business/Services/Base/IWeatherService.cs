using MagicMirror.Business.Models;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public interface IWeatherService : IService<WeatherModel>
    {
        Task<WeatherModel> GetWeatherModel(string city);
    }
}