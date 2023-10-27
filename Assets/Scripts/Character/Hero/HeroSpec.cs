using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace CryptoQuest.Character.Hero
{
    /// <summary>
    /// Runtime hero data, use this to save game
    ///
    /// User should have bunch of Heroes in their profile
    ///
    /// Use the <see cref="AttributeSystemBehaviour"/> to get the runtime stats
    ///
    /// <para>I'm not saving skill/abilities here instead I'll get it at run time using the <see cref="UnitSO"/> from combination of
    /// <see cref="Elemental"/> and <see cref="CharacterClass"/></para>
    /// </summary>
    [Serializable]
    public struct HeroSpec
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public UnitSO Unit { get; set; }
        [field: SerializeField] public float Experience { get; set; }
        [field: SerializeField] public Equipments Equipments { get; set; }
        public bool IsValid() => Unit != null;

        /// <summary>
        /// Use this to make sure equipped equipment has valid hero id 
        /// </summary>
        public void ValidateEquipments()
        {
            foreach (var slot in Equipments.Slots)
            {
                var equipment = slot.Equipment;
                if (!equipment.IsValid() || equipment.IsEquipped) continue;
                equipment.SetEquippedHeroUnitId(Id);
            }
        }
    }
}