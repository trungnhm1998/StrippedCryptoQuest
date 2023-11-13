using System;
using System.Collections;
using CryptoQuest.AbilitySystem.Abilities.Conditions;
using CryptoQuest.Item;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    /// <summary>
    /// Base data class for consumable such as herb, potion, etc.
    /// </summary>
    public class ConsumeItemAbilityWithContext : ConsumeItemAbility
    {
        [SerializeField] private GameplayEffectContext _context;
        public GameplayEffectContext Context => _context;
        protected override GameplayAbilitySpec CreateAbility() => new ConsumableAbilitySpecWithContext(this);
    }

    public class ConsumableAbilitySpecWithContext : ConsumableAbilitySpec
    {
        private readonly ConsumeItemAbilityWithContext _def;
        public ConsumableAbilitySpecWithContext(ConsumeItemAbilityWithContext def) : base(def)
            => _def = def;

        protected override GameplayEffectSpec CreateEffectSpec(GameplayEffectDefinition effectDef)
        {
            var contextHandle = new GameplayEffectContextHandle(_def.Context);
            return Owner.MakeOutgoingSpec(effectDef, contextHandle);
        }
    }
}