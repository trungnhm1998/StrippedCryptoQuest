using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class MockUpgradeEquipment : IUpgradeEquipment
    {
        public EquipmentInfo Equipment => _equipment;
        public LocalizedString DisplayName => _equipment.Data.DisplayName;
        public Sprite Icon => _equipment.Data.EquipmentType.Icon;
        public Sprite Rarity => _equipment.Rarity.Icon;
        public AssetReferenceT<Sprite> Illustration => _equipment.Data.Image;
        public float Cost => _equipment.Price; //TODO: Cost to upgrade is fake data, Will remove this class when have real data
        public int Level => _equipment.Level;
        private EquipmentInfo _equipment;

        public MockUpgradeEquipment(EquipmentInfo equipmentInfo)
        {
            _equipment = equipmentInfo;
        }
    }
}