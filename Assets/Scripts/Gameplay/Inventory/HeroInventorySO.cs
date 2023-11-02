using System;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public struct SaveAttributeValue
    {
        [JsonProperty("attribute")]
        public string AttributeGuid;

        [JsonProperty("value")]
        public float Value;
    }

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

        [JsonProperty("stats")]
        public List<SaveAttributeValue> Attributes;
    }

    public class HeroInventorySO : ScriptableObject
    {
        [field: SerializeField] public List<Hero> OwnedHeroes { get; set; } = new();
    }
}