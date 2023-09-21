using System;
using System.Collections;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay.Inventory.Items;
using IndiGames.Core.Events.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.Ability
{
    /// <summary>
    /// Base data class for consumable such as herb, potion, etc.
    /// </summary>
    public class ConsumeItemAbility : AbilityScriptableObject
    {
        [SerializeField] private VoidEventChannelSO _showConfirmationUI;

        /// <summary>
        /// Derived class should raise event to show correct UI if there is any
        ///
        /// currently we have behavior:
        /// - Single target hero
        /// - Ocarina with special UI flow
        /// - Target all hero in party (This doesn't have UI flow yet)
        /// </summary>
        public void Consuming() => _showConfirmationUI.RaiseEvent();

        protected override GameplayAbilitySpec CreateAbility() => new ConsumableAbilitySpec();

        public ConsumableAbilitySpec GetAbilitySpec(ConsumableInfo consumable,
            AbilitySystemBehaviour owner) => new(consumable, owner, this);
    }

    /// <summary>
    /// Base logic class for consumable ability
    ///
    /// Ocarina should derived from this for logic
    /// </summary>
    public class ConsumableAbilitySpec : GameplayAbilitySpec
    {
        private readonly ConsumableInfo _consumable;
        private readonly AbilitySystemBehaviour _owner;
        private readonly ConsumeItemAbility _def;

        public ConsumableAbilitySpec() { }

        public ConsumableAbilitySpec(ConsumableInfo consumable, AbilitySystemBehaviour target,
            ConsumeItemAbility def)
        {
            _consumable = consumable;
            _owner = target;
            _def = def;
        }

        public override bool CanActiveAbility()
        {
            _owner.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var hp);
            return base.CanActiveAbility() && hp.CurrentValue > 0;
        }

        protected override IEnumerator OnAbilityActive()
        {
            throw new NotImplementedException();
        }
    }
}