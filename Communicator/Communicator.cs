using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Gitloy.Services.Common.Communicator
{
    public class Communicator : ICommunicator
    {
        public static string ConfigSection { get; set; } = "RabbitMQ";

        private IBus _bus;
        public IBus Bus
        {
            get
            {
                if (!IsConnected)
                {
                    _logger.LogWarning("Communicator is not connected. Connecting process started.");
                    Connect();
                }

                return _bus;
            }
            private set => _bus = value;
        }

        public bool IsConnected => (_bus != null && _bus.IsConnected);
        private readonly IConfiguration _config;
        private readonly ILogger<Communicator> _logger;
        
        public Communicator(IConfiguration config, ILogger<Communicator> logger)
        {
            _config = config;
            _logger = logger;
        }
        
        public void Connect()
        {
            if (IsConnected)
            {
                _logger.LogDebug("Trying to connect but communicator is already connected.");
                return;
            }
            
            ValidateConfig();
            
            _bus?.Dispose();
            Bus = RabbitHutch.CreateBus(BuildConnectionString());

            if (!Bus.IsConnected)
            {
                _bus?.Dispose();
                throw new Exception($"Failed to build communicator. No connection to broker.");
            }
            
            _logger.LogInformation("Building communicator finished.");
        }

        public void Disconnect()
        {
            if (!IsConnected)
            {
                _logger.LogDebug("Trying to disconnect but communicator is already disconnected.");
                return;
            }
            
            _bus?.Dispose();
            _bus = null;
        }

        private void ValidateConfig()
        {
            if (!_config.GetSection(ConfigSection).Exists())
                throw new ArgumentException($"[Communicator] Config section '{ConfigSection}' " +
                                            $"containing EasyNetQ connection parameters not found.");
        }

        private string BuildConnectionString()
        {
            string connectionString = string.Empty;
            
            var easyNetQConfig = _config.GetSection(ConfigSection).Get<EasyNetQConfig>();
            
            foreach (var property in easyNetQConfig.GetType().GetProperties())
            {
                string name = property.Name;
                string value = property.GetValue(easyNetQConfig).ToString();
                connectionString += $"{name}={value};";
            }

            connectionString = connectionString.TrimEnd(';');
            return connectionString;
        }

        public void Dispose()
        {
            Bus?.Dispose();
        }
    }
}