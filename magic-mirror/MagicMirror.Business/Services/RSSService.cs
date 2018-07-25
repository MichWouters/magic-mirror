using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Repos;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public class RssService : IRSSService
    {
        private const string OFFLINEMODELNAME = "RssOfflineModel.json";
        private readonly IRssRepo _repo;

        public RssService(IRssRepo repo)
        {
            _repo = repo;
        }

        public async Task<RssModel> GetModelAsync()
        {
            try
            {
                // Get entity from Repository.
                var entity = await _repo.GetEntityAsync();

                // Map entity to model.
                var model = MapEntityToModel(entity);

                return model;
            }
            catch (HttpRequestException) { throw; }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to retrieve RSS Model", ex);
            }
        }


        private RssModel GenerateOfflineModel()
        {
            string[] funnyPages = new string[]
               {
                    "Amerikaanse wapenfabrikanten betreuren overlijden van trouwe klant",
                    "Grondwettelijk Hof stelt voor 'Rich meet Beautiful open voor cougars, milfs en andere sugar mommy’s'",
                    "Man die na vijftien jaar uit coma ontwaakt, vraagt om meteen opnieuw in slaap gebracht te worden",
                    "Geheime liefdescorrespondentie Donald Trump en Kim Jong-Un gelekt",
                    "Leven na de dood bewezen, kerkbezoek sterk gestegen",
                    "Grote terugroepactie Tedepi V9",
                    "Slimme spiegel in ontwikkeling, Experts maken zich zorgen",
                    "Studie toont aan dat minder meer is. Min of meer",
                    "Man red kat uit boom, buren verrukt",
                    "Trein komt op tijd aan. Wetenschappers voor raadsel"
               };

            var model = new RssModel();
            var items = new ObservableCollection<RssItem>();

            foreach (string page in funnyPages)
            {
                items.Add(new RssItem { Title = page });
            }

            model.Items = items;
            return model;
        }
    }
}