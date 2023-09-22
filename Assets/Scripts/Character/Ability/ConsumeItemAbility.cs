using System;
using System.Collections;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Item;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.Ability
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
            var canActiveAbility = base.CanActiveAbility() && CanApplyAttributeModifier(_consumable.Data.Effect);
            if (!canActiveAbility)
                Debug.Log($"Consume {_consumable.Data} failed on {Owner.name}");
            return canActiveAbility;
        }

        protected override IEnumerator OnAbilityActive()
        {
            foreach (var tag in _def.CancelEffectWithTags)
            {
                Debug.Log($"Cancel effect with tag {tag}");
                Owner.EffectSystem.ExpireEffectWithTag(tag);
            }

            var effect = _consumable.Data.Effect;
            var effectSpec = Owner.MakeOutgoingSpec(effect);
            Owner.ApplyEffectSpecToSelf(effectSpec);
            yield break;
        }

        private bool CanApplyAttributeModifier(GameplayEffectDefinition def)
        {
            Owner.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var hp);
            if (hp.CurrentValue <= 0)
            {
                Debug.Log($"Can't apply attribute modifier because {Owner.name} is dead");
                return false;
            }

            for (int i = 0; i < def.EffectDetails.Modifiers.Length; i++)
            {
                var modDef = def.EffectDetails.Modifiers[i];

                // Only worry about additive.  Anything else passes.
                if (modDef.ModifierType != EAttributeModifierType.Add) continue;
                if (modDef.Attribute == null) continue;
                if (!Owner.AttributeSystem.TryGetAttributeValue(modDef.Attribute, out var attributeValue)) continue;
                
                if (modDef.Value < 0) return true; // TODO: Debug using bomb on self

                var attributeWithCapped = modDef.Attribute as AttributeWithMaxCapped;
                if (attributeWithCapped == null) continue;
                Owner.AttributeSystem.TryGetAttributeValue(attributeWithCapped.CappedAttribute, out var cappedValue);
                if (attributeValue.CurrentValue < cappedValue.CurrentValue)
                {
                    return true;
                }
            }

            Debug.Log($"All attribute already at max value");

            return false;
        }
    }
}