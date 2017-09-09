using AutoMapper;
using AutoMapper.Configuration;
using MagicMirror.Business.Configuration;
using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities;
using MagicMirror.DataAccess.Repos;
using System;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public abstract class ServiceBase<T, Y> : IService<T>
        where T : IModel
        where Y : IEntity
    {
        protected IMapper Mapper;
        protected IApiRepo<Y> _repo;
        protected SearchCriteria _criteria;

        protected ServiceBase()
        {
            SetUpMapperConfiguration();
        }

        // Child classes must implement abstract methods.
        public abstract Task<T> GetModelAsync();

        /// <summary>
        /// Calculate the model's fields which cannot be resolved using Automapper.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected abstract T CalculateMappedValues(T model);

        // Child classes can override virtual methods.
        /// <summary>
        /// Map Entity to Business Model using AutoMapper
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual T MapEntityToModel(IEntity entity)
        {
            T model = Mapper.Map<T>(entity);
            return model;
        }

        /// <summary>
        /// Define a reference to the automapper configuration class
        /// so Visual Studio knows which fields need be mapped between the Models and Entities
        /// </summary>
        private void SetUpMapperConfiguration()
        {
            var baseMappings = new MapperConfigurationExpression();
            baseMappings.AddProfile<AutoMapperConfiguration>();
            var config = new MapperConfiguration(baseMappings);
            Mapper = new Mapper(config);
        }

        protected virtual void SetUpServiceConfiguration()
        {
            throw new NotImplementedException();
        }
    }
}