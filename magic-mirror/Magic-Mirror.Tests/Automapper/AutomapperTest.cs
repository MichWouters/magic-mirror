using AutoMapper;
using MagicMirror.Business.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MagicMirror.Tests.Automapper
{

    public class AutomapperTest
    {
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            Mapper.Initialize(m => m.AddProfile<AutoMapperConfiguration>());
            Mapper.AssertConfigurationIsValid();
        }
    }
}
