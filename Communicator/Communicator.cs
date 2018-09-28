using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using EasyNetQ;
using Microsoft.Extensions.Configuration;

namespace Gitloy.Services.Common.Communicator
{
    public class Communicator : IDisposable
    {
        #region Singleton
        
        private static readonly Lazy<Communicator> LazyInstance = 
            new Lazy<Communicator>(() => new Communicator());

        public static Communicator Instance => LazyInstance.Value;

        #endregion
        
        protected static IConfiguration Configuration { get; private set; }
        
        public static string ConfigJsonFile { get; set; } = "GitloyCommunicator";
        public static string ConfigSection { get; set; } = "RabbitMQ";
        
        public IBus Bus { get; private set; }
        
        private Communicator()
        {
            NormalizeConfFilename();
            BuildConfiguration();
            BuildBus();
        }

        private void BuildBus()
        {
            Bus = RabbitHutch.CreateBus(BuildConnectionString());
        }

        private string BuildConnectionString()
        {
            string connectionString = string.Empty;
            var easyNetQConfig = Configuration.GetSection(ConfigSection).Get<EasyNetQConfig>();
            
            foreach (var property in easyNetQConfig.GetType().GetProperties())
            {
                string name = property.Name;
                string value = property.GetValue(easyNetQConfig).ToString();
                connectionString += $"{name}={value};";
            }

            connectionString = connectionString.TrimEnd(';');
            return connectionString;
        }

        private void BuildConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConfigJsonFile, optional: false, reloadOnChange: false)
                .Build();
        }

        private void NormalizeConfFilename()
        {
            ConfigJsonFile = ConfigJsonFile.Trim().Replace(".json", "") + ".json";
        }

        public void Dispose()
        {
            Bus?.Dispose();
        }
    }
}