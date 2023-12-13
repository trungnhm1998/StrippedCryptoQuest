using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Item.Equipment;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Evolve
{
    public class EvolvableEquipmentData : IEvolvableEquipment
    {
        public IEquipment Equipment { get; set; }

        public Sprite Icon => Equipment.Type.Icon;

        public LocalizedString LocalizedName => Equipment.Prefab.DisplayName;

        public int Level { get; set; }

        public int Stars { get; set; }

        public int Gold { get; set; }

        public float Metad { get; set; }

        public Sprite Rarity => Equipment.Rarity.Icon;

        public int Rate { get; set; }

        public int AfterStars { get; set; }

        public int MinLevel { get; set; }

        public int MaxLevel { get; set; }
    }
}
