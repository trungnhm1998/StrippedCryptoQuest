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
        public EquipmentPrefab Prefab { get; set; }

        public LocalizedString DisplayName => Prefab.DisplayName;

        public Sprite Icon => Prefab.EquipmentType.Icon;

        public Sprite Rarity => Equipment.Rarity.Icon;

        public AssetReferenceT<Sprite> Illustration => Prefab.Image;

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
                yield return _equipmentPrefabDatabase.LoadDataById(equipment.Data.PrefabId);
                var prefab = _equipmentPrefabDatabase.GetDataById(equipment.Data.PrefabId);

                IUpgradeEquipment equipmentData = new UpgradeEquipmentData
                {
                    Equipment = equipment,
                    Prefab = prefab,
                };
                if (equipment.Level < equipment.Data.MaxLevel)
                    _equipmentData.Add(equipmentData);
            }
        }

        private void OnDisable()
        {
            foreach (var equipment in _equipmentData)
                _equipmentPrefabDatabase.ReleaseDataById(equipment.Equipment.PrefabId);
        }
    }
}