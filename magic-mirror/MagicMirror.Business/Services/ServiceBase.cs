using AutoMapper;
using AutoMapper.Configuration;
using MagicMirror.Business.Configuration;

namespace MagicMirror.Business.Services
{
    public abstract class ServiceBase
    {
        protected IMapper Mapper;

        protected ServiceBase()
        {
            SetUpMapperConfiguration();
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
    }
}