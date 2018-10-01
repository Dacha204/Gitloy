using System.Threading.Tasks;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Core.Handlers
{
    public interface IGitEventHandler<in TEvent> where TEvent : class
    {
        void Handle(TEvent data);
        Task HandleAsync(TEvent data);
    }
}