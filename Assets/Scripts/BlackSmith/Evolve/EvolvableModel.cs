using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Item.Equipment;
using UnityEngine.Localization;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;

namespace CryptoQuest.BlackSmith.Evolve
{
    public class EvolveableEquipmentData : IEvolvableEquipment
    {
        public EvolveableEquipmentData(EquipmentInfo equipment)
        {
            Equipment = equipment;
        }

        public EquipmentInfo Equipment { get; private set; }

        public Sprite Icon => Equipment.Type.Icon;

        public LocalizedString LocalizedName => Equipment.Prefab.DisplayName;

        public int Level => Equipment.Level;

        public int Stars { get; set; }

        public int Gold { get; set; }

        public float Metad { get; set; }

        public Sprite Rarity => Equipment.Rarity.Icon;

        public int Rate { get; set; }
    }

    public class EvolvableModel : MonoBehaviour, IEvolvableModel
    {
        [SerializeField] private EquipmentPrefabDatabase _equipmentPrefabDatabase;
        private List<IEvolvableEquipment> _evolvableEquipments;
        public List<IEvolvableEquipment> EvolvableEquipments => _evolvableEquipments;

        public IEnumerator CoGetData(InventorySO inventory)
        {
            _evolvableEquipments = new();
            var equipments = new List<EquipmentInfo>();
            equipments.AddRange(inventory.NftEquipments);
            equipments.AddRange(inventory.Equipments);
            foreach (var equipment in equipments)
            {
                IEvolvableEquipment equipmentData = new EvolveableEquipmentData(equipment);
                // TODO: check which equipments are evolvable
                // if (equipment.Level < equipment.Data.MaxLevel)
                _evolvableEquipments.Add(equipmentData);
            }

            yield break;
        }
    }
}
