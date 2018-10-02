using System;
using Gitloy.BuildingBlocks.Messages.Data.Validation;

namespace Gitloy.BuildingBlocks.Messages.IntegrationEvents
{
    /// <summary>
    /// Raised when Github PingEvent is received for specific integration entry.
    /// Means that user has correctly set webhook for his github repo.
    /// </summary>
    public class IntegrationPingEvent : IntegrationEvent
    {
        protected override void ValidateMe()
        {
        }
    }
}