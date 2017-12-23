using MagicMirror.DataAccess.Compliments;
using System;

namespace MagicMirror.Business.Services
{
    public class ComplimentService : IComplimentService
    {
        public string GenerateCompliment()
        {
            var compRepo = new ComplimentsRepo().GetHardCodedCompliments();
            int randomCompliment = new Random().Next(compRepo.Count);
            string compliment = compRepo[randomCompliment];

            return compliment;
        }
    }
}