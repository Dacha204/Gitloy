using System;
using System.Net;
using Gitloy.BuildingBlocks.Messages.Data.Validation;

namespace Gitloy.BuildingBlocks.Messages.IntegrationEvents
{
    public abstract class IntegrationEvent : IValidate
    {
        #region ValidationHelpers

        protected static void ValidateString(string param, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"{param} is null or empty.");
        }

        protected static void ValidatePort(int port)
        {
            if (IPEndPoint.MinPort <= port && port >= IPEndPoint.MaxPort)
                throw new ArgumentException("Port is invalid.");
        }

        #endregion
        
        public Guid EventGuid { get; set; }
        public Guid IntegrationGuid { get; set; }
        public DateTime DateTime { get; set; }
        
        protected IntegrationEvent()
        {
            EventGuid = Guid.NewGuid();
            DateTime = DateTime.Now;
        }

        public ValidationResult Validate()
        {
            try
            {
                ValidateMe();
                return ValidationResult.Valid;
            }
            catch(Exception e)
            {
                return ValidationResult.Invalid(e.Message);
            }
        }

        protected abstract void ValidateMe();
    }
}