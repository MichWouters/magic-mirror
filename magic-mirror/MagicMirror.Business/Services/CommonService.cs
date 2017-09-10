using MagicMirror.DataAccess.Compliments;
using System;
using System.Threading.Tasks;

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
    }
}