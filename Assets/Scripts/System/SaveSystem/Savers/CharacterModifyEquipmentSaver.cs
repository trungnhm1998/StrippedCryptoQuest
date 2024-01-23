using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item.Equipment;
using IndiGames.Core.Events;
using IndiGames.Core.Events.ScriptableObjects;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    [Serializable]
    public class EquipmentMapSerializeObject
    {
        public int Id;
        public int CharacterId;
        public ESlot OccupiedSlot;
    }

    public class SaveEquipmentAction : ActionBase { }

    public class CharacterModifyEquipmentSaver : SagaBase<SaveEquipmentAction>
    {
        [SerializeField] private PartySO _party;
        [SerializeField] private SaveSystemSO _saveSystem;
        [SerializeField] private VoidEventChannelSO _forceSaveEventChannel;

        /// <summary>
        /// O(n) time complexity every time we save the equipment TODO: Optimize
        /// </summary>
        protected override void HandleAction(SaveEquipmentAction _)
        {
            var maps = new List<EquipmentMapSerializeObject>();
            foreach (var partySlot in _party)
            {
                foreach (var equipmentSlot in partySlot.EquippingItems.Slots)
                {
                    if (equipmentSlot.IsValid() == false) continue;
                    maps.Add(new EquipmentMapSerializeObject()
                    {
                        Id = equipmentSlot.Equipment.Id,
                        CharacterId = partySlot.Hero.Id,
                        OccupiedSlot = equipmentSlot.Type
                    });
                }
            }

            var json = JsonConvert.SerializeObject(maps);
            _saveSystem[SerializeKeys.EQUIPMENT_MAP] = json;
            _forceSaveEventChannel.RaiseEvent();
        }
    }
}