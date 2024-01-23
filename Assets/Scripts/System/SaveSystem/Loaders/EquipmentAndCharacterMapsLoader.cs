using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System.SaveSystem.Savers;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class EquipmentAndCharacterMapsLoader : Loader
    {
        [SerializeField] private SaveSystemSO _saveSystem;
        [SerializeField] private EquipmentInventory _inventory;
        [SerializeField] private PartySO _party;

        public override void Load()
        {
            if (!_saveSystem.SaveData.TryGetValue(SerializeKeys.EQUIPMENT_MAP, out var json)) return;
            // using json utils cause not deserializable
            var maps = JsonConvert.DeserializeObject<List<EquipmentMapSerializeObject>>(json);
            foreach (var partySlot in _party)
            {
                partySlot.EquippingItems.Clear();
                foreach (var map in maps)
                {
                    if (map.CharacterId != partySlot.Hero.Id) continue;
                    var findEquipment = _inventory.FindEquipment(map.Id);
                    findEquipment.AttachCharacterId = map.CharacterId;
                    partySlot.EquippingItems.Slots.Add(new EquipmentSlot()
                    {
                        Equipment = findEquipment,
                        Type = map.OccupiedSlot
                    });
                }
            }
        }
    }
}