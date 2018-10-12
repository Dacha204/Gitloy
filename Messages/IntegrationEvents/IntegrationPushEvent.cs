using System;
using Gitloy.BuildingBlocks.Messages.Data.Validation;

namespace Gitloy.BuildingBlocks.Messages.IntegrationEvents
{
    public enum ResultStatus
    {
        Unknown,
        Successful,
        Failed
    }
    
    /// <summary>
    /// Raised when new Git PushEvent is received via webhook.
    /// </summary>
    public class IntegrationPushEvent : IntegrationEvent
    {
        public ResultStatus ResultStatus { get; set; }
        public string ResultMessage { get; set; }
        public string Details { get; set; }

        //TODO
//        public string LastCommit { get; set; }
        
        public IntegrationPushEvent()
        {
            ResultStatus = ResultStatus.Unknown;
            ResultMessage = string.Empty;
            Details = string.Empty;
        }

        protected override void ValidateMe()
        {
            if (ResultMessage == null)
                throw new ArgumentException($"{nameof(ResultMessage)} is null");

            if (Details == null)
                throw new ArgumentException($"{nameof(Details)} is null");
        }
    }
}