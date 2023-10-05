using Newtonsoft.Json;

namespace CryptoQuest.Networking.Menu.DimensionBox
{
    public class Data
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("metad")]
        public float Metad { get; set; }
    }
    public class MetadResponseData
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("gold")]
        public int Gold { get; set; }

        [JsonProperty("diamond")]
        public int Diamond { get; set; }

        [JsonProperty("soul")]
        public int Soul { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }
}
