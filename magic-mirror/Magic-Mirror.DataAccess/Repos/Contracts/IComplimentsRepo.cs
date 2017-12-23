using MagicMirror.DataAccess.Entities.Entities;

namespace MagicMirror.DataAccess.Compliments
{
    public interface IComplimentsRepo
    {
        ComplimentEntity Get(string keyword);
    }
}