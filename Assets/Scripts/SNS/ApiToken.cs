using Newtonsoft.Json;
using System;

namespace CryptoQuest.SNS
{
    [Serializable]
    public class ApiToken
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expires")]
        public DateTime Expires { get; set; }
    }
}
