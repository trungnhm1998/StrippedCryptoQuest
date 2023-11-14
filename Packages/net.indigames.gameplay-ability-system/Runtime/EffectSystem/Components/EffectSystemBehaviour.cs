using System.Collections.Generic;
using System.Linq;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.Components
{
    /// <summary>
    /// Wrapper around the <see cref="AttributeSystemBehaviour"/> to handle the effects
    /// for every applied effect find all of it modifiers and add it to the attribute in the <see cref="AttributeSystemBehaviour"/>
    /// </summary>
    [RequireComponent(typeof(AttributeSystemBehaviour))]
    public class EffectSystemBehaviour : MonoBehaviour
    {
        [SerializeField] private bool _useUpdate;

        /// <summary>
        /// Currently there are no restrictions on add a new effect to the system except
        /// when using <see cref="ApplyEffectToSelf"/> which will check <see cref="GameplayEffectSpec.CanApply"/>
        /// </summary>
        private readonly List<ActiveGameplayEffect> _appliedEffects = new();

        public IReadOnlyList<ActiveGameplayEffect> AppliedEffects => _appliedEffects;
        private AbilitySystemBehaviour _owner;

        public AbilitySystemBehaviour Owner
        {
            get => _owner;
            set => _owner = value;
        }

        private AttributeSystemBehaviour _attributeSystem;

        public AttributeSystemBehaviour AttributeSystem
        {
            get => _attributeSystem;
            set => _attributeSystem = value;
        }

        protected virtual void Awake()
        {
            _owner = GetComponent<AbilitySystemBehaviour>();
            _attributeSystem = GetComponent<AttributeSystemBehaviour>();
        }

        /// <summary>
        /// Will create a new AbstractEffect from EffectScriptableObject (data)
        /// this will update the Owner of the effect to this AbilitySystem
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        public GameplayEffectSpec GetEffect(GameplayEffectDefinition def)
            => def.CreateEffectSpec(Owner, Owner.MakeEffectContext());

        // TODO: Move to AbilityEffectBehaviour
        /// <summary>
        /// AbilitySystemComponent.cpp::ApplyGameplayEffectSpecToSelf::line 730
        ///
        /// Create an active effect spec, apply into the system and update the attribute accordingly in this frame
        /// </summary>
        /// <param name="inSpec"></param>
        /// <returns></returns>
        public ActiveGameplayEffect ApplyEffectToSelf(GameplayEffectSpec inSpec)
        {
            if (inSpec == null) return new ActiveGameplayEffect();

            inSpec.Target = Owner;

            if (!inSpec.CanApply()) return new ActiveGameplayEffect();

            inSpec.CalculateModifierMagnitudes();
            var activeEffectSpecification = inSpec.CreateActiveEffectSpec();
            if (activeEffectSpecification is InstantActiveEffectPolicy)
            {
                activeEffectSpecification.ExecuteActiveEffect();
                return activeEffectSpecification;
            }

            _appliedEffects.Add(activeEffectSpecification);
            Owner.TagSystem.AddTags(activeEffectSpecification.GrantedTags);
            UpdateAttributeSystemModifiers();

            AttemptRemoveActiveEffectsOnEffectApplication(activeEffectSpecification);

            var instigator = inSpec.Context.Get().InstigatorAbilitySystemComponent;
            OnGameplayEffectAppliedToSelf(instigator, inSpec);
            if (instigator != null) instigator.EffectSystem.OnGameplayEffectAppliedToTarget(this, inSpec);

            return activeEffectSpecification;
        }

        private void OnGameplayEffectAppliedToTarget(EffectSystemBehaviour effectSystemBehaviour,
            GameplayEffectSpec inSpec) { }

        private void OnGameplayEffectAppliedToSelf(AbilitySystemBehaviour instigator, GameplayEffectSpec inSpec) { }

        /// <summary>
        /// When an effect being applied to the system, we need to check if there's any effect that can be removed
        /// </summary>
        /// <param name="activeEffectSpecification">applying effect</param>
        /// TODO: Implement
        private void AttemptRemoveActiveEffectsOnEffectApplication(ActiveGameplayEffect activeEffectSpecification) { }

        private ActiveGameplayEffect FindStackableActiveGameplayEffect(GameplayEffectSpec inSpec)
        {
            if (!inSpec.Def.IsStack || inSpec.Def.Policy is InstantPolicy) return null;
            return _appliedEffects.FirstOrDefault(appliedEffect => appliedEffect.Spec.Def == inSpec.Def);
        }

        public void ExpireEffectWithTagImmediately(TagScriptableObject tag)
        {
            ExpireEffectWithTag(tag);
            UpdateAttributeModifiersUsingAppliedEffects();
        }

        public void ExpireEffectWithTag(TagScriptableObject tag)
        {
            foreach (var appliedEffect in _appliedEffects)
            {
                if (appliedEffect.Expired) continue;
                if (!appliedEffect.HasTag(tag)) continue;
                appliedEffect.IsActive = false;
            }
        }

        /// <summary>
        /// Remove the effect from the system
        /// We should also remove the effect's modifiers from the attribute
        /// </summary>
        public virtual void RemoveEffect(GameplayEffectSpec effectSpec)
        {
            if (effectSpec == null || !effectSpec.IsValid())
            {
                Debug.LogWarning("Try remove invalid effect");
                return;
            }

            for (int i = _appliedEffects.Count - 1; i >= 0; i--)
            {
                var effect = _appliedEffects[i];
                if (effect.IsValid() == false || effectSpec.CompareTo(effect.Spec) == 1)
                {
                    RemoveEffectAtIndex(i);
                }
            }

            // after remove the effect from system we need to update the attribute modifiers
            UpdateAttributeModifiersUsingAppliedEffects();
        }

        private void Update()
        {
            if (_useUpdate) UpdateAttributeModifiersUsingAppliedEffects();
        }

        private void OnDestroy()
        {
            for (var i = _appliedEffects.Count - 1; i >= 0; i--)
            {
                var effect = _appliedEffects[i];
                effect.OnRemoved();
            }
        }

        public virtual void UpdateAttributeModifiersUsingAppliedEffects()
        {
            UpdateAttributeSystemModifiers();
            UpdateEffects();
            RemoveExpiredEffects();
            _attributeSystem.UpdateAttributeValues();
        }

        /// <summary>
        /// 1. Remove all modifier from attribute value
        /// 2. Add all modifiers from all active effects
        /// </summary>
        public virtual void UpdateAttributeSystemModifiers()
        {
            _attributeSystem.ResetAttributeModifiers();
            foreach (var effect in _appliedEffects.Where(effect => !effect.Expired))
            {
                effect.Spec.CalculateModifierMagnitudes();
                effect.ExecuteActiveEffect();
            }
        }

        private void UpdateEffects()
        {
            for (var index = 0; index < _appliedEffects.Count; index++)
            {
                var effectContainer = _appliedEffects[index];
                if (effectContainer != null && !effectContainer.Expired)
                    effectContainer.Update(Time.deltaTime);
            }
        }

        public void RemoveExpiredEffects()
        {
            for (var i = _appliedEffects.Count - 1; i >= 0; i--)
            {
                var effect = _appliedEffects[i];
                if (effect != null && effect.IsValid() && !effect.Expired) continue;
                RemoveEffectAtIndex(i);
            }
        }

        /// <summary>
        /// Tests if all modifiers in this GameplayEffect will leave the attribute > 0.f
        /// </summary>
        /// <param name="effectDef"></param>
        /// <returns></returns>
        public bool CanApplyAttributeModifiers(GameplayEffectDefinition effectDef)
        {
            var spec = new GameplayEffectSpec();
            spec.InitEffect(effectDef, _owner);

            for (int i = 0; i < spec.Modifiers.Length; i++)
            {
                var modDef = effectDef.EffectDetails.Modifiers[i];

                // Only worry about additive.  Anything else passes.
                if (modDef.OperationType != EAttributeModifierOperationType.Add) continue;
                if (modDef.Attribute == null) continue;

                if (!_attributeSystem.TryGetAttributeValue(modDef.Attribute, out var attributeValue)) continue;
                spec.CalculateModifierMagnitudes();
                var modSpec = spec.Modifiers[i];
                var hasEnoughResource = attributeValue.CurrentValue + modSpec.GetEvaluatedMagnitude() < 0;
                if (hasEnoughResource) return false;
            }

            return true;
        }

        private void RemoveEffectAtIndex(int index)
        {
            var effect = _appliedEffects[index];
            _appliedEffects.RemoveAt(index);
            if (effect?.Spec == null) return;
            Owner.TagSystem.RemoveTags(effect.GrantedTags);
            effect.OnRemoved();
        }

        [SerializeField] private List<PreEffectExecuteEvent> _preEffectExecuteEvents = new();

        /// <summary>
        /// Called just before modifying the value of an attribute. AttributeSet can make additional modifications here. Return true to continue, or false to throw out the modification.
        /// Note this is only called during an 'execute'. E.g., a modification to the 'base value' of an attribute. It is not called during an application of a GameplayEffect, such as a 5 ssecond +10 movement speed buff.
        /// </summary>
        public bool PreGameplayEffectExecute(GameplayEffectModCallbackData executeData)
        {
            var ignore = true;
            foreach (var preEffectExecuteEvent in _preEffectExecuteEvents)
            {
                if (preEffectExecuteEvent == null) continue;
                ignore |= preEffectExecuteEvent.Execute(executeData);
            }

            return ignore;
        }

        public void PostGameplayEffectExecute(GameplayEffectModCallbackData executeData) { }
    }
}