using MagicMirror.DataAccess.Compliments;
using System;

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
    }
}