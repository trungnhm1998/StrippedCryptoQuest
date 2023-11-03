using System;
using CryptoQuest.Character.Hero;
using UnityEngine;

namespace CryptoQuest.Gameplay.PlayerParty
{
    /// <summary>
    /// Wrapper of hero spec in a party, only in party heroes able to equip items
    /// </summary>
    [Serializable]
    public class PartySlotSpec
    {
        [field: SerializeField] public HeroSpec Hero { get; set; }
        [field: SerializeField] public Equipments EquippingItems { get; set; }

        /// <summary>
        /// Use this to make sure equipped equipment has valid hero id 
        /// </summary>
        public void ValidateEquipments()
        {
            foreach (var slot in EquippingItems.Slots)
            {
                var equipment = slot.Equipment;
                if (!equipment.IsValid() || equipment.IsEquipped) continue;
                equipment.SetEquippedHeroUnitId(Hero.Id);
            }
        }
    }

    public class MockParty : ScriptableObject
    {
        [SerializeField] private PartySlotSpec[] _heroSpecs;
        public PartySlotSpec[] GetParty() => _heroSpecs;
        public void SetParty(PartySlotSpec[] newSpecs) => _heroSpecs = newSpecs;
    }
}