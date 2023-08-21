using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
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
        public List<EffectSpecificationContainer> AppliedEffects { get; } = new();

        private AbilitySystemBehaviour _owner;
        public AbilitySystemBehaviour Owner => _owner;

        private AttributeSystemBehaviour _attributeSystem;
        private IEffectApplier _effectApplier;

        // TODO: Could use a factory here
        protected IEffectApplier EffectAppliers => _effectApplier ??= new DefaultEffectApplier(Owner);


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
        /// <param name="origin"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public AbstractEffect GetEffect(EffectScriptableObject effectSO, object origin, AbilityParameters parameters)
            => effectSO.GetEffect(Owner, origin, parameters);

        // TODO: Move to AbilityEffectBehaviour
        public AbstractEffect ApplyEffectToSelf(AbstractEffect inEffectSpec)
        {
            if (inEffectSpec == null || !inEffectSpec.CanApply()) return NullEffect.Instance;

            inEffectSpec.SetTarget(Owner);
            inEffectSpec.Accept(EffectAppliers);
            return inEffectSpec;
        }

        /// <summary>
        /// Remove the effect from the system
        /// We should also remove the effect's modifiers from the attribute
        /// </summary>
        /// <param name="abstractEffect"></param>
        public virtual void RemoveEffect(AbstractEffect abstractEffect)
        {
            for (int i = AppliedEffects.Count - 1; i >= 0; i--)
            {
                var effect = AppliedEffects[i];
                if (abstractEffect.EffectSO != effect.EffectSpec.EffectSO) continue;

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
        /// Update the attribute system modifiers using the applied effects, but first reset all attribute to their base values
        /// </summary>
        protected virtual void UpdateAttributeSystemModifiers()
        {
            // Reset all attribute to their base values
            _attributeSystem.ResetAttributeModifiers();
            foreach (var effect in AppliedEffects)
            {
                AddModifiersToAttributeWithEffect(effect);
            }
        }

        protected void AddModifiersToAttributeWithEffect(EffectSpecificationContainer effect)
        {
            if (effect.Expired) return;

            foreach (var effectModifierDetail in effect.Modifiers)
            {
                AddAttributeToSystemIfNotExists(effectModifierDetail.Attribute);
                _attributeSystem.TryAddModifierToAttribute(
                    effectModifierDetail.Modifier,
                    effectModifierDetail.Attribute,
                    effect.ModifierType);
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
            if (_attributeSystem.HasAttribute(attribute, out _)) return;
            _attributeSystem.AddAttributes(attribute);
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
    }
}