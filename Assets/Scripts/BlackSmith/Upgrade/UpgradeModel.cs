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
        public UpgradeEquipmentData(EquipmentInfo equipment)
        {
            Equipment = equipment;
        }

        public EquipmentInfo Equipment { get; private set; }

        public LocalizedString DisplayName => Equipment.DisplayName;

        public Sprite Icon => Equipment.Config.EquipmentType.Icon;

        public Sprite Rarity => Equipment.Rarity.Icon;

        public AssetReferenceT<Sprite> Illustration => Equipment.Config.Image;

        // TODO: Get cost from database #2417
        public float Cost { get; set; }

        public int Level => Equipment.Level;
    }

    public class UpgradeModel : MonoBehaviour, IUpgradeModel
    {
        [SerializeField] private EquipmentPrefabDatabase _equipmentPrefabDatabase;
        private List<IUpgradeEquipment> _equipmentData;
        public List<IUpgradeEquipment> Equipments => _equipmentData;

        public IEnumerator CoGetData(InventorySO inventory)
        {
            _equipmentData = new();
            var listEquipment = inventory.Equipments;
            foreach (var equipment in listEquipment)
            {
                if (equipment.Config == null)
                {
                    yield return _equipmentPrefabDatabase.LoadDataById(equipment.Data.PrefabId);
                    equipment.Config = _equipmentPrefabDatabase.GetDataById(equipment.Data.PrefabId);
                }

                IUpgradeEquipment equipmentData = new UpgradeEquipmentData(equipment);
                if (equipment.Level < equipment.Data.MaxLevel)
                    _equipmentData.Add(equipmentData);
            }
        }
    }
}