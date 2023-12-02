using System.Collections.Generic;
using System.Linq;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using UnityEngine;
using UnityEngine.Localization;

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

    public interface IEvolvableModel
    {
        public List<IEquipment> GetEvolableEquipments();
    }

    public class EvolvableModel : MonoBehaviour, IEvolvableModel
    {
        [SerializeField] private InventorySO _inventory;

        /// <summary>
        /// Only nft equipments can be evolved
        /// </summary>
        /// <returns>equipments that can be evolve</returns>
        public List<IEquipment> GetEvolableEquipments() =>
            _inventory.NftEquipments.Where(CanEvolve).Cast<IEquipment>().ToList();

        private static bool CanEvolve(IEquipment equipment)
        {
            // return equipment.Level == equipment.Data.MaxLevel;
            // this equipment should have at least 1 "material" same equipment also max lvl
            // using equipment.Data.Id
            return true;
        }
    }
}