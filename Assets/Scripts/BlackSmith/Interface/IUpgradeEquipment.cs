
using CryptoQuest.Item;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Interface
{
    public interface IUpgradeEquipment
    {
        public ItemInfo Item { get; }
        public LocalizedString DisplayName { get; }
        public AssetReferenceT<Sprite> Icon { get; }
        public float Cost { get; }
    }
}