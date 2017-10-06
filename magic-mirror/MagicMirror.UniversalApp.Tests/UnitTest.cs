using Microsoft.VisualStudio.TestTools.UnitTesting;
using MagicMirror.UniversalApp.Services;
using System.Threading.Tasks;
using Xunit;

namespace MagicMirror.UniversalApp.Tests
{
    public class SettingsServiceTests
    {
        private SettingsService _service;

        public SettingsServiceTests()
        {
            _service = new SettingsService();
        }

        [Fact]
        public async void  Save_Settings_Success()
        {
            _service.SaveSettings();
        }
    }
}
