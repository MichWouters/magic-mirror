using Microsoft.VisualStudio.TestTools.UnitTesting;
using MagicMirror.UniversalApp.Services;
using System.Threading.Tasks;

namespace MagicMirror.UniversalApp.Tests
{
    [TestClass]
    public class SettingsServiceTests
    {
        private SettingsService _service;

        public SettingsServiceTests()
        {
            _service = new SettingsService();
        }

        [TestMethod]
        public async Task Save_Settings_Success()
        {
            await _service.SaveSettings();
        }
    }
}
