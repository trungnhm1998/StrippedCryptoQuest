using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Gameplay.PlayerParty.Helper;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using IndiGames.Core.Common;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.BlackSmith.Evolve
{
    public interface IEvolvableModel
    {
        void Init();

        List<IEquipment> GetEvolableEquipments();

        List<IEquipment> FilterByInfos(IEvolvableInfo[] infos);

        List<IEquipment> FilterByEquipment(IEquipment equipment);
    }

    public class EvolvableModel : MonoBehaviour, IEvolvableModel
    {
        [FormerlySerializedAs("_inventory")] [SerializeField] private EquipmentInventory _equipmentInventory;

        private IPartyController _partyController;
        private List<IEquipment> _equipments;

        public void Init() => _equipments = GetAvailableEquipments().Where(CanEvolve).Cast<IEquipment>().ToList();

        /// <summary>
        /// Only nft equipments can be evolved
        /// </summary>
        /// <returns>equipments that can be evolve</returns>
        public List<IEquipment> GetEvolableEquipments() => _equipments;

        private List<IEquipment> GetAvailableEquipments()
        {
            _partyController ??= ServiceProvider.GetService<IPartyController>();
            // Equipped equipment will be removed from the list
            return _equipmentInventory.Equipments.Where(e => (e.IsNft && !e.IsEquipped())).ToList();
        }

        /// Only items with max level can be evolved
        private static bool CanEvolve(IEquipment equipment) => equipment.Level >= equipment.Data.MaxLevel;

        public List<IEquipment> FilterByInfos(IEvolvableInfo[] infos) => ProcessFilterByInfos(infos);

        public List<IEquipment> FilterByEquipment(IEquipment equipment) => ProcessFilterByEquipment(equipment);

        protected virtual List<IEquipment> ProcessFilterByInfos(IEvolvableInfo[] info)
        {
            _equipments = _equipments
                .Where(e => info.Any(i => i.Rarity == e.Data.Rarity.ID && i.BeforeStars == e.Data.Stars)).ToList();
            return _equipments;
        }

        protected virtual List<IEquipment> ProcessFilterByEquipment(IEquipment equipment)
        {
            _equipments = _equipments.Where(e => e.Data.ID == equipment.Data.ID && e != equipment).ToList();
            return _equipments;
        }
    }
}