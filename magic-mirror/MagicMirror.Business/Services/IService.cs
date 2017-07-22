using MagicMirror.Business.Models;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public interface IService<T> where T : Model
    {
        Task<T> GetModelAsync();

        T MapEntityToModel();

        T CalculateMappedValues();
    }
}
