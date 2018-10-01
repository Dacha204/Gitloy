using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Gitloy.Services.WebhookAPI.GithubPayloads
{
    public static class GithubPayloadExtractor
    {
        public static GithubPayload Extract(HttpRequest request)
        {
            request.Headers.TryGetValue("X-GitHub-Event", out var strEvent);
            request.Headers.TryGetValue("Content-type", out var content);
            
            // ReSharper disable once InconsistentNaming
            string PayloadText;

            if (content != "application/json")
            {
                throw new Exception("Invalid content type. Expected application/json");
            }

            using (var reader = new StreamReader(request.Body, Encoding.UTF8))
            {
                PayloadText = reader.ReadToEnd();
            }

            var payloadObject = ConvertObject(strEvent, PayloadText);

            return new GithubPayload(payloadObject.GetType(), payloadObject, strEvent);
        }

        private static object ConvertObject(string @event, string json)
        {
            switch (@event)
            {
                case "ping":
                    return JsonConvert.DeserializeObject<GithubPingEvent>(json);
                case "push":
                    return JsonConvert.DeserializeObject<GithubPushEvent>(json);

                default:
                    throw new NotImplementedException(
                        $"Event Type: `{@event}` is not implemented.");
            }
        }
    }
}