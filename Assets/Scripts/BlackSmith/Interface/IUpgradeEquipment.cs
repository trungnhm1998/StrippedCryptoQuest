
using CryptoQuest.BlackSmith.ScriptableObjects;
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
        EquipmentInfo Equipment { get; }
        LocalizedString DisplayName { get; }
        Sprite Icon { get; }
        Sprite Rarity { get; }
        AssetReferenceT<Sprite> Illustration { get; }
        int Level { get;}
        
        float GetCost(int currentLevel, int levelToUpgrade);
    }
}