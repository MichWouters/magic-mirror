using MagicMirror.Business.Models;

namespace MagicMirror.Business.Services
{
    public interface IFileWriterService
    {
        UserSettings ReadSettings(string folder, string fileName);

        void SaveSettings(string path, string USERSETTINGS, UserSettings settings);
    }
}