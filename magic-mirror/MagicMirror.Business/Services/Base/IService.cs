using MagicMirror.DataAccess.Entities.Entities;

namespace MagicMirror.Business.Services
{
    public interface IService<T>
    {
        T MapFromEntity(Entity entity);
    }
}