﻿using System;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Gameplay;
using CryptoQuest.Item.Equipment;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace CryptoQuest.Character.Hero
{
    [Serializable]
    public struct EquipmentSlotData
    {
        public EquipmentSlot.EType Type;
        public EquipmentData Equipment;
    }

    [Serializable]
    public struct HeroSpecData
    {
        public int Id;
        public string UnitSOGuid;
        public float Experience;
        public string EquipmentSlots;
    }

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
    public class HeroSpec
    {
        /// <summary>
        /// Main should not have id always 0
        /// </summary>
        [field: SerializeField] public int Id { get; set; }

        [field: SerializeField] public float Experience { get; set; }
        [field: SerializeField] public Elemental Elemental { get; set; }
        [field: SerializeField] public CharacterClass Class { get; set; }
        [field: SerializeField] public StatsDef Stats { get; set; }
        [field: SerializeField] public Origin Origin { get; set; }
        public bool IsValid() => Elemental != null && Class != null && Stats.IsValid();
    }
}