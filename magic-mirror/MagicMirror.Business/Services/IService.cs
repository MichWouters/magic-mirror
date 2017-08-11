using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities;
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
        /// Map an Entity object to a Model object
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T MapEntityToModel(Entity entity);

        /// <summary>
        /// Calculate the model's fields which cannot be resolved using Automapper.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        T CalculateMappedValues(T model);

        
    }
}