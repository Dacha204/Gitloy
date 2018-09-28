using System;
using System.IO;
using Xunit;
using Gitloy.Services.Common.Communicator;

namespace Gitloy.Services.Common.Tests.CommunicatorTests
{
    //TODO: Make real tests
    public class CommunicatorTests : IDisposable
    {
        [Fact]
        public void FirstCall_ValidDefaultConfigFileWithAllProperties()
        {
            var comm = Communicator.Communicator.Instance;
            Assert.NotNull(comm.Bus);
        }

        [Fact]
        public void FirstCall_InvalidDefaultFile_ThrowsFileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(() =>
            {
                Communicator.Communicator.ConfigJsonFile = "RandomFile.json";
                var comm = Communicator.Communicator.Instance;
            });
        }

        [Fact]
        public void FirstCall_ValidDefaultConfigFilePartialProperties_LoadsDefaultParams()
        {
            Communicator.Communicator.ConfigJsonFile = "PartialConfig.json";
            var comm = Communicator.Communicator.Instance;
            Assert.NotNull(comm.Bus);
        }

        [Fact]
        public void FirstCall_SpecificDeploymentEnvironment_Connected()
        {
            Communicator.Communicator.ConfigJsonFile = "DevelopmentSpecific.json";
            var comm = Communicator.Communicator.Instance;
            Assert.True(comm.Bus.IsConnected);
        }

        public void Dispose()
        {
            Communicator.Communicator.Instance.Dispose();
        }
    }
}