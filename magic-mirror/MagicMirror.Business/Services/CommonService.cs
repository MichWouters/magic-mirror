using MagicMirror.DataAccess.Compliments;
using System;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public class CommonService : ISettingsService
    {
        public string GenerateCompliment()
        {
            var compRepo = new ComplimentsRepo().GetCompliments();
            int randomCompliment = new Random().Next(compRepo.Count);
            string compliment = compRepo[randomCompliment];

            return compliment;
        }

        public void ReadSettings()
        {
            throw new NotImplementedException();
        }

        public Task SaveSettings()
        {
            throw new NotImplementedException();
        }
    }


    public class Settings
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}