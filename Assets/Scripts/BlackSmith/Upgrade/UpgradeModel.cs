using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.BlackSmith.ScriptableObjects;
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

        public int Level => Equipment.Level;

        public CostByRarity CostData { get; set; }

        public float GetCost(int currentLevel, int toLevel)
        {
            // Index is imported base on master data in this format
            var currentIndex = currentLevel - 1;
            var toIndex = toLevel - 1;
            int totalCost = 0;

            for (int i = currentIndex; i < toIndex; i++)
            {
                totalCost += CostData.Costs[i];
            }
            return totalCost;
        }
    }

    public class UpgradeModel : MonoBehaviour, IUpgradeModel
    {
        [SerializeField] private UpgradeCostDatabase _costDatabase;
        private Dictionary<int, CostByRarity> _costDataDict = new();

        private List<IUpgradeEquipment> _equipmentData;
        public List<IUpgradeEquipment> Equipments => _equipmentData;

        private void Awake()
        {
            foreach (var data in _costDatabase.CostData)
            {
                _costDataDict.TryAdd(data.RarityID, data);
            }
        }

        public IEnumerator CoGetData(InventorySO inventory)
        {
            _equipmentData = new();
            var equipments = new List<EquipmentInfo>();
            equipments.AddRange(inventory.NftEquipments);
            equipments.AddRange(inventory.Equipments);
            foreach (var equipment in equipments)
            {
                var equipmentData = new UpgradeEquipmentData
                {
                    Equipment = equipment,
                };
                if (equipment.Level >= equipment.Data.MaxLevel) continue;

                _costDataDict.TryGetValue(equipment.Rarity.ID, out var data);
                equipmentData.CostData = data;
                _equipmentData.Add(equipmentData);
            }

            yield break;
        }
    }
}