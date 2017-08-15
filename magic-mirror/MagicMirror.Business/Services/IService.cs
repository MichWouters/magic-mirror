using MagicMirror.Business.Models;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public interface IService<T> where T : Model
    {
        /// <summary>
        /// Get the model without automapped and calculated fields.
        /// </summary>
        /// <returns></returns>
        Task<T> GetModelAsync();

        /// <summary>
        /// Calculate the model's fields which cannot be resolved using Automapper.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        T CalculateMappedValues(T model);
    }
}