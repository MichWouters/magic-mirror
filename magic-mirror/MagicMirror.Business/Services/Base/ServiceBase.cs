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
        /// Provide dummy data when no internet connection can be established
        /// </summary>
        public abstract Task<T> GetOfflineModelAsync();

        /// <summary>
        /// Save dummy data for when no internet connection can be established.
        /// </summary>
        public abstract Task SaveOfflineModel(T model);

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
        /// Map Business Model to Entity using AutoMapper
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual Y MapModelToEntity(T model)
        {
            Y entity = Mapper.Map<Y>(model);
            return entity;
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