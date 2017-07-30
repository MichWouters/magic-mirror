using System;
using MagicMirror.DataAccess.Compliments;

namespace MagicMirror.Business.Services
{
    public class CommonService
    {
        public string GenerateCompliment()
        {
            var compRepo = new ComplimentsRepository().GetCompliments();
            int randomCompliment = new Random().Next(compRepo.Count);
            string compliment = compRepo[randomCompliment];

            return compliment;
        }

        public string GetNameOfDay()
        {
            var today = DateTime.Today.DayOfWeek.ToString();
            return today;
        }

        public string GetDateString()
        {
            var today = DateTime.Today.Date.ToString("D");
            return today;
        }
    }
}
