using System;
using System.Collections.Generic;
using CryptoQuest.Character.Ability;
using CryptoQuest.Gameplay.Character;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace CryptoQuest.Character.Hero
{
    /// <summary>
    /// Runtime hero data, use this to save game
    ///
    /// Use the <see cref="AttributeSystemBehaviour"/> to get the runtime stats
    /// </summary>
    [Serializable]
    public class HeroSpec : CharacterInformation<HeroDef, HeroSpec>
    {
        /// <summary>
        /// Use this render skill
        /// </summary>
        [field: SerializeField] public List<CastableAbility> LearnedAbilities { get; set; }

        [field: SerializeField] public float Experience { get; set; }
        [field: SerializeField, ReadOnly] public int Level { get; set; }
        [field: SerializeField] public Equipments Equipments { get; private set; }
    }
}