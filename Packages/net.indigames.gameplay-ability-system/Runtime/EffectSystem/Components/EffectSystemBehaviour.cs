using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.EffectApplier;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.Components
{
    /// <summary>
    /// Wrapper around the <see cref="AttributeSystemBehaviour"/> to handle the effects
    /// for every applied effect find all of it modifiers and add it to the attribute in the <see cref="AttributeSystemBehaviour"/>
    /// </summary>
    public partial class EffectSystemBehaviour : MonoBehaviour
    {
        public List<ActiveEffectSpecification> AppliedEffects { get; } = new();

        private AbilitySystemBehaviour _owner;
        public AbilitySystemBehaviour Owner => _owner;

        private AttributeSystemBehaviour _attributeSystem;
        private IEffectApplier _effectApplier;

        private IEffectApplier EffectAppliers => _effectApplier ??= new DefaultEffectApplier(Owner);


        public void InitSystem(AbilitySystemBehaviour owner)
        {
            _owner = owner;
            _attributeSystem = owner.AttributeSystem;
        }

        /// <summary>
        /// Will create a new AbstractEffect from EffectScriptableObject (data)
        /// this will update the Owner of the effect to this AbilitySystem
        /// </summary>
        /// <param name="effectSO"></param>
        /// <returns></returns>
        public GameplayEffectSpec GetEffect(EffectScriptableObject effectSO)
            => effectSO.CreateEffectSpec(Owner);

        // TODO: Move to AbilityEffectBehaviour
        /// <summary>
        /// AbilitySystemComponent.cpp::ApplyGameplayEffectSpecToSelf::line 730
        /// </summary>
        /// <param name="inEffectSpecSpec"></param>
        /// <returns></returns>
        public ActiveEffectSpecification ApplyEffectToSelf(GameplayEffectSpec inEffectSpecSpec)
        {
            if (inEffectSpecSpec == null || !inEffectSpecSpec.CanApply()) return new ActiveEffectSpecification();

            inEffectSpecSpec.Target = Owner;
            return inEffectSpecSpec.Accept(EffectAppliers);
        }

        /// <summary>
        /// Remove the effect from the system
        /// We should also remove the effect's modifiers from the attribute
        /// </summary>
        public virtual void RemoveEffect(GameplayEffectSpec effectSpec)
        {
            for (int i = AppliedEffects.Count - 1; i >= 0; i--)
            {
                var effect = AppliedEffects[i];
                if (effectSpec.Def != effect.EffectSpec.Def) continue;

                AppliedEffects.RemoveAt(i);
                break;
            }

            // after remove the effect from system we need to update the attribute modifiers
            ForceUpdateAttributeSystemModifiers();
        }

        private void Update()
        {
            UpdateAttributeModifiersUsingAppliedEffects();
        }

        public void UpdateAttributeModifiersUsingAppliedEffects()
        {
            UpdateAttributeSystemModifiers();
            UpdateEffects();
            RemoveExpiredEffects();
        }

        /// <summary>
        /// Force the system to check all effects and update their status
        /// </summary>
        public void ForceUpdateAttributeSystemModifiers()
        {
            UpdateAttributeSystemModifiers();
            _attributeSystem.UpdateAttributeValues();
        }

        /// <summary>
        /// 1. Reset all attributes to their base value
        /// 2. Add all modifiers from all active effects
        /// </summary>
        protected virtual void UpdateAttributeSystemModifiers()
        {
            _attributeSystem.ResetAttributeModifiers();
            for (var index = 0; index < AppliedEffects.Count; index++)
            {
                var effect = AppliedEffects[index];
                if (effect.Expired) continue;
                AddModifiersToAttributeWithEffect(effect);
            }
        }

        private void AddModifiersToAttributeWithEffect(ActiveEffectSpecification activeEffect)
        {
            if (activeEffect.Expired) return;

            for (var index = 0; index < activeEffect.ComputedModifiers.Count; index++)
            {
                var computedModifier = activeEffect.ComputedModifiers[index];

                AddAttributeToSystemIfNotExists(computedModifier.Attribute);
                _attributeSystem.TryAddModifierToAttribute(
                    computedModifier.Modifier,
                    computedModifier.Attribute,
                    activeEffect.ModifierType);
            }
        }

        /// <summary>
        /// The case is we have an effect with modifier want to affect an attribute that is not in the system yet
        ///
        /// e.g. Modifier to increase gold drop rate, but the attribute system does not have gold drop rate attribute.
        /// We can either add the attribute to the system or this method would add it for us. only at runtime
        /// </summary>
        private void AddAttributeToSystemIfNotExists(AttributeScriptableObject attribute)
        {
            _attributeSystem.AddAttribute(attribute);
        }

        protected virtual void UpdateEffects()
        {
            for (var index = 0; index < AppliedEffects.Count; index++)
            {
                var effectContainer = AppliedEffects[index];
                if (!effectContainer.Expired)
                    effectContainer.Update(Time.deltaTime);
            }
        }

        protected virtual void RemoveExpiredEffects()
        {
            for (var i = AppliedEffects.Count - 1; i >= 0; i--)
            {
                var effect = AppliedEffects[i];
                if (!effect.Expired) continue;

                AppliedEffects.RemoveAt(i);
                Owner.TagSystem.RemoveTags(effect.GrantedTags);
            }
        }

        /// <summary>
        /// Tests if all modifiers in this GameplayEffect will leave the attribute > 0.f
        /// </summary>
        /// <param name="effectDef"></param>
        /// <returns></returns>
        public bool CanApplyAttributeModifiers(EffectScriptableObject effectDef)
        {
            var spec = new GameplayEffectSpec();
            spec.InitEffect(effectDef);
            spec.CalculateModifierMagnitudes();

            for (int i = 0; i < spec.Modifiers.Length; i++)
            {
                var modDef = effectDef.EffectDetails.Modifiers[i];
                var modSpec = spec.Modifiers[i];

                // Only worry about additive.  Anything else passes.
                if (modDef.ModifierType != EAttributeModifierType.Add) continue;
                if (modDef.Attribute == null) continue;

                if (!_attributeSystem.TryGetAttributeValue(modDef.Attribute, out var attributeValue)) continue;
                var hasEnoughResource = attributeValue.CurrentValue + modSpec.GetEvaluatedMagnitude() < 0;
                if (hasEnoughResource) return false;
            }

            return true;
        }
    }
}