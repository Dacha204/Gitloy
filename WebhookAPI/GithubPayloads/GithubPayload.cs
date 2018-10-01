using System;

namespace Gitloy.Services.WebhookAPI.GithubPayloads
{
    public class GithubPayload
    {
        public Type Type { get; private set; }
        public object Result { get; private set; }
        public string Event { get; private set; }

        public GithubPayload(Type type, object result, string @event)
        {
            Type = type;
            Result = result;
            Event = @event;
        }
    }
}