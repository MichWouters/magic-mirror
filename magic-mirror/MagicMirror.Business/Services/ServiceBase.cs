using AutoMapper;
using AutoMapper.Configuration;
using MagicMirror.Business.Configuration;

namespace MagicMirror.Business.Services
{
    public abstract class ServiceBase
    {
        private IMapper _mapper;

        public ServiceBase()
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
            _mapper = new Mapper(config);
        }
    }
}
