using CryptoQuest.BlackSmith.Interface;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Evolve
{
    public class EvolvableMockData : IEvolvableData
    {
        public Sprite Icon { get; private set; }
        public LocalizedString LocalizedName { get; private set; }
        public int Level { get; private set; }
        public int Stars { get; private set; }
        public int Gold { get; private set; }
        public float Metad { get; private set; }
        public Sprite Rarity { get; private set; }
        public int Rate { get; private set; }

        public EvolvableMockData(Sprite icon, LocalizedString name, int level, int stars, int gold, float metad, Sprite rarity, int rate)
        {
            Icon = icon;
            LocalizedName = name;
            Level = level;
            Stars = stars;
            Gold = gold;
            Metad = metad;
            Rarity = rarity;
            Rate = rate;
        }
    }
}