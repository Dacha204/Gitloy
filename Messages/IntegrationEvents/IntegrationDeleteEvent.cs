using Gitloy.BuildingBlocks.Messages.Data.Validation;

namespace Gitloy.BuildingBlocks.Messages.IntegrationEvents
{
    /// <summary>
    /// Raised when integration is removed.
    /// </summary>
    public class IntegrationDeleteEvent : IntegrationEvent
    {
        protected override void ValidateMe()
        {
        }
    }
}