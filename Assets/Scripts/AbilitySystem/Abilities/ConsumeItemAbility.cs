using System;
using System.Collections;
using CryptoQuest.AbilitySystem.Abilities.Conditions;
using CryptoQuest.Item;
using CryptoQuest.Item.Consumable;
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
    public class ConsumeItemAbility : AbilityScriptableObject
    {
        /// <summary>
        /// Expires any effects with these tags when this ability is activated
        /// </summary>
        [field: SerializeField] public TagScriptableObject[] CancelEffectWithTags { get; private set; } =
            Array.Empty<TagScriptableObject>();

        [field: SerializeReference, ReferenceEnum] 
        public IAbilityCondition[] Conditions { get; private set; } = Array.Empty<IAbilityCondition>();

        private void OnValidate()
        {
            for (int i = 0; i < Conditions.Length; i++)
            {
                if (Conditions[i] != null) continue;
                Conditions[i] = new AlwaysTrue();
            }
        }

        protected override GameplayAbilitySpec CreateAbility() => new ConsumableAbilitySpec(this);
    }

    /// <summary>
    /// Base logic class for consumable ability
    ///
    /// Ocarina should derived from this for logic
    /// </summary>
    public class ConsumableAbilitySpec : GameplayAbilitySpec
    {
        private readonly ConsumeItemAbility _def;
        private ConsumableInfo _consumable;
        public ConsumableAbilitySpec(ConsumeItemAbility def) => _def = def;
        public void SetConsumable(ConsumableInfo consumable) => _consumable = consumable;

        public override bool CanActiveAbility()
        {
            var canActiveAbility = base.CanActiveAbility() && CanPassAllCondition();
            if (!canActiveAbility)
                Debug.Log($"Consume {_consumable.Data} failed on {Owner.name}");
            return canActiveAbility;
        }

        protected override IEnumerator OnAbilityActive()
        {
            foreach (var tag in _def.CancelEffectWithTags)
            {
                Owner.EffectSystem.ExpireEffectWithTagImmediately(tag);
            }

            var effect = _consumable.Data.Effect;
            var effectSpec = CreateEffectSpec(effect);
            Owner.ApplyEffectSpecToSelf(effectSpec);
            EndAbility();
            yield break;
        }

        protected virtual GameplayEffectSpec CreateEffectSpec(GameplayEffectDefinition effectDef)
        {
            return Owner.MakeOutgoingSpec(effectDef);
        }

        private bool CanPassAllCondition()
        {
            foreach (var condition in _def.Conditions)
            {
                var isConditionPass = condition.IsPass(new AbilityConditionContext(Owner, _consumable.Data.Effect)); 
                if (!isConditionPass) return false;
            }

            return true;
        }
    }
}