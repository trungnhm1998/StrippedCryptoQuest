using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.BlackSmith.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using IndiGames.Core.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UpgradeModel : MonoBehaviour, IUpgradeModel
    {

        private List<Item.Equipment.IEquipment> _equipmentData;
        public List<Item.Equipment.IEquipment> Equipments => _equipmentData;
        private IPartyController _partyController;

        public IEnumerator CoGetData(InventorySO inventory)
        {
            _equipmentData = new();
            var equipments = new List<IEquipment>();
            equipments.AddRange(GetEquippingEquipments());
            equipments.AddRange(inventory.NftEquipments);
            equipments.AddRange(inventory.Equipments);
            
            foreach (var equipment in equipments)
            {
                if (equipment.Level >= equipment.Data.MaxLevel) continue;
                _equipmentData.Add(equipment);
            }

            yield break;
        }

        private List<IEquipment> GetEquippingEquipments()
        {
            List<IEquipment> equipments = new();
            _partyController ??= ServiceProvider.GetService<IPartyController>();

            foreach (var slot in _partyController.Slots)
            {
                if (!slot.IsValid()) continue;

                var hero = slot.HeroBehaviour;
                foreach (var equipSlot in hero.GetEquipments().Slots)
                {
                    var equipping = equipSlot.Equipment;
                    if (equipping == null || !equipping.IsValid())
                        continue;

                    // Prevent add dual weilding
                    if (!equipments.Contains(equipping))
                        equipments.Add(equipping);
                }
            }

            return equipments;
        }
    }
}