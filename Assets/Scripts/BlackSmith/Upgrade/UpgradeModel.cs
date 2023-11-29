using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UpgradeEquipmentData : IUpgradeEquipment
    {
        public EquipmentInfo Equipment { get; set; }

        public LocalizedString DisplayName => Equipment.Prefab.DisplayName;

        public Sprite Icon => Equipment.Type.Icon;

        public Sprite Rarity => Equipment.Rarity.Icon;

        public AssetReferenceT<Sprite> Illustration => Equipment.Prefab.Image;

        // TODO: Get cost from database #2417
        public float Cost { get; set; }

        public int Level => Equipment.Level;
    }

    public class UpgradeModel : MonoBehaviour, IUpgradeModel
    {
        private List<IUpgradeEquipment> _equipmentData;
        public List<IUpgradeEquipment> Equipments => _equipmentData;

        public IEnumerator CoGetData(InventorySO inventory)
        {
            _equipmentData = new();
            var listEquipment = inventory.Equipments;
            foreach (var equipment in listEquipment)
            {
                IUpgradeEquipment equipmentData = new UpgradeEquipmentData
                {
                    Equipment = equipment,
                };
                if (equipment.Level < equipment.Data.MaxLevel)
                    _equipmentData.Add(equipmentData);
            }

            yield break;
        }
    }
}