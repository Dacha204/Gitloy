using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gitloy.Services.Common.Communicator
{
    public class Communicator : ICommunicator
    {
        private readonly IOptions<EasyNetQOptions> _options;
        
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
        private readonly ILogger<Communicator> _logger;
        
        public Communicator(ILogger<Communicator> logger, IOptions<EasyNetQOptions> options)
        {
            _logger = logger;
            _options = options;
        }
        
        public void Connect()
        {
            if (IsConnected)
            {
                _logger.LogDebug("Trying to connect but communicator is already connected.");
                return;
            }
            
            _bus?.Dispose();
            var connectionString = BuildConnectionString();
            Bus = RabbitHutch.CreateBus(connectionString);

            if (!IsConnected)
            {
                _bus?.Dispose();
                _logger.LogDebug($"Failed to build communicator. No connection. Connection string: {connectionString}");
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

        private string BuildConnectionString()
        {
            string connectionString = string.Empty;

            var easyNetQConfig = _options.Value;
            
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