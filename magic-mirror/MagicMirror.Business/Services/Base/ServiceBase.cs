using AutoMapper;
using AutoMapper.Configuration;
using MagicMirror.Business.Configuration;
using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities;
using MagicMirror.DataAccess.Repos;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public abstract class ServiceBase<T, Y> : IApiService<T> where T : IModel
                                                             where Y : IEntity
    {
        protected IMapper Mapper;
        protected IApiRepo<Y> _repo;
        protected UserSettings _criteria;

        protected ServiceBase()
        {
            SetUpMapperConfiguration();
        }

        // Child classes MUST implement abstract methods.
        public abstract Task<T> GetModelAsync();

        /// <summary>
        /// Calculate the model's fields which cannot be resolved using Automapper.
        /// </summary>
        protected abstract T CalculateUnMappableValues(T model);

        /// <summary>
        /// Retrieve unmodified entity from data layer
        /// </summary>
        protected abstract Task<Y> GetEntityAsync();

        // Child classes CAN override virtual methods.
        /// <summary>
        /// Map Entity to Business Model using AutoMapper
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual T MapEntityToModel(Y entity)
        {
            T model = Mapper.Map<T>(entity);
            return model;
        }

        /// <summary>
        /// Define a reference to the automapper configuration class so Visual Studio knows which fields need be mapped between the Models and Entities
        /// </summary>
        private void SetUpMapperConfiguration()
        {
            var baseMappings = new MapperConfigurationExpression();
            baseMappings.AddProfile<AutoMapperConfiguration>();
            var config = new MapperConfiguration(baseMappings);
            Mapper = new Mapper(config);
        }
    }
}