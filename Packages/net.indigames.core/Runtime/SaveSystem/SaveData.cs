using System;
using Newtonsoft.Json;

namespace IndiGames.Core.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public string playerName = "";

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