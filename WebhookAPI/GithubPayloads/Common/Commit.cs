using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gitloy.Services.WebhookAPI.GithubPayloads.Common
{
    public class Commit
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("tree_id")]
        public string TreeId { get; set; }

        [JsonProperty("distinct")]
        public bool Distinct { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("committer")]
        public Committer Committer { get; set; }

        [JsonProperty("added")]
        public IList<string> Added { get; set; }

        [JsonProperty("removed")]
        public IList<object> Removed { get; set; }

        [JsonProperty("modified")]
        public IList<object> Modified { get; set; }
    }
}