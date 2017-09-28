using Acme.Generic;
using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Compliments;
using Newtonsoft.Json;
using System;

namespace MagicMirror.Business.Services
{
    public class CommonService
    {
        public string GenerateCompliment()
        {
            var compRepo = new ComplimentsRepo().GetCompliments();
            int randomCompliment = new Random().Next(compRepo.Count);
            string compliment = compRepo[randomCompliment];

            return compliment;
        }

        public UserSettings ReadSettings(string folder, string fileName)
        {
            string json = FileWriter.ReadFromFile(folder, fileName);
            var result = JsonConvert.DeserializeObject<UserSettings>(json);

            return result;
        }

        public void SaveSettings(string path, string USERSETTINGS, UserSettings settings)
        {
            string json = JsonConvert.SerializeObject(settings);
            FileWriter.WriteJsonToFile(json, USERSETTINGS, path);
        }
    }
}