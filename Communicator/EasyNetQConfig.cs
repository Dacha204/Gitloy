using System;
using System.Reflection;

namespace Gitloy.Services.Common.Communicator
{
    internal class EasyNetQConfig
    {
        public string Host { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RequestedHeartbeat { get; set; }
        public int PrefetchCount { get; set; }
        public bool PublisherConfirms { get; set; }
        public bool PersistentMessages { get; set; }
        public string Product { get; set; }
        public string Platform { get; set; }
        public int Timeout { get; set; }

        public EasyNetQConfig()
        {
            Host = "localhost";
            VirtualHost = "/";
            Username = "admin";
            Password = "password";
            RequestedHeartbeat = 10;
            PrefetchCount = 50;
            PublisherConfirms = false;
            PersistentMessages = true;
            Product = Assembly.GetExecutingAssembly().GetName().Name;
            Platform = Environment.MachineName;
            Timeout = 10;
        }
    }
}