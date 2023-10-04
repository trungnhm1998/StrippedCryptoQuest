using System;
using Newtonsoft.Json;

namespace IndiGames.Core.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public const string DEFAULT_PLAYER_NAME = "New Player";

        public string playerName = DEFAULT_PLAYER_NAME;

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void LoadFromJson(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }
    }
}