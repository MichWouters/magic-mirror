using MagicMirror.Business.Models;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public interface IApiService<T> where T : IModel
    {
        /// <summary>
        /// Get the model without automapped and calculated fields.
        /// </summary>
        /// <returns></returns>
        Task<T> GetModelAsync();
    }
}