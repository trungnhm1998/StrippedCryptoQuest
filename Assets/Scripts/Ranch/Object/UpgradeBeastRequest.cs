using System;
using Newtonsoft.Json;

namespace CryptoQuest.Ranch.Object
{
    [Serializable]
    public struct UpgradeBeastRequest
    {
        [JsonProperty("beforeLv")]
        public int BeforeLevel;

        [JsonProperty("beast")]
        public BeastUpgrade Beast;

        [Serializable]
        public class BeastUpgrade
        {
            [JsonProperty("level")]
            public int Level;

            [JsonProperty("beastId")]
            public string BeastId;

            [JsonProperty("id")]
            public int Id;
        }
    }
}