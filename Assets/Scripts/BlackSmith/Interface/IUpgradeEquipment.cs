
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Interface
{
    public interface IUpgradeEquipment
    {
        public EquipmentInfo Equipment { get; }
        public LocalizedString DisplayName { get; }
        public Sprite Icon { get; }
        public Sprite Rarity { get; }
        public AssetReferenceT<Sprite> Illustration { get; }
        public float Cost { get; }
        public int Level { get;}
    }
}