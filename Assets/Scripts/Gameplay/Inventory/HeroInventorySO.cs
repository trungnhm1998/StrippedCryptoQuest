using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public struct Hero
    {
        [JsonProperty("id")]
        public int Id;

        [JsonProperty("element")]
        public string Element;

        [JsonProperty("class")]
        public string Job; // using class might conflicts with C# keyword

        [JsonProperty("experience")]
        public float Experience;
    }

    public class HeroInventorySO : ScriptableObject
    {
        [field: SerializeField] public List<Hero> OwnedHeroes { get; set; } = new();
    }
}