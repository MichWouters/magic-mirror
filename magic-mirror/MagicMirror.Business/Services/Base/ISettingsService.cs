using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public interface ISettingsService
    {
        Task SaveSettings();

        void ReadSettings();
    }
}
