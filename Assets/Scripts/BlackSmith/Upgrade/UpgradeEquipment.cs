using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menu;
using CryptoQuest.System;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UpgradeEquipment : IUpgradeEquipment
    {
        public EquipmentInfo Equipment => _equipment;
        public LocalizedString DisplayName => _equipment.Data.DisplayName;
        public Sprite Icon => _equipment.Data.EquipmentType.Icon;
        public Sprite Rarity => _equipment.Rarity.Icon;
        public AssetReferenceT<Sprite> Illustration => _equipment.Data.Image;
        public float Cost => _equipment.Price;
        public int Level => _equipment.Level;
        private EquipmentInfo _equipment;

        public UpgradeEquipment(EquipmentInfo equipmentInfo)
        {
            _equipment = equipmentInfo;
        }
    }
}