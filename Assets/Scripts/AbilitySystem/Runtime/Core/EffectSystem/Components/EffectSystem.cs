using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Indigames.AbilitySystem
{
    public class EffectSystem : MonoBehaviour
    {
        public List<EffectSpecificationContainer> AppliedEffects = new List<EffectSpecificationContainer>();

        protected AbilitySystem _owner;
        public AbilitySystem Owner => _owner;

        private AttributeSystem _attributeSystem;
        private IEffectApplier _effectApplier;
        protected IEffectApplier EffectAppliers
        {
            get
            {
                if (_effectApplier == null)
                {
                    _effectApplier = new EffectApplier(Owner);
                }

                return _effectApplier;
            }
        }

        private void Start()
        {
            _attributeSystem = Owner.AttributeSystem;
        }        

        public void InitSystem(AbilitySystem owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Will create a new AbstractEffect from EffectScriptableObject (data)
        /// this will update the Owner of the effect to this SkillSystem
        /// </summary>
        /// <param name="effectSO"></param>
        /// <param name="origin"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public AbstractEffect GetEffect(EffectScriptableObject effectSO, object origin, AbilityParameters parameters)
        {
            UpdateAttributeSystemModifiers();
            return effectSO.GetEffect(Owner, origin, parameters);
        }

        public AbstractEffect ApplyEffectToSelf(AbstractEffect inEffectSpec)
        {
            if (inEffectSpec == null || !inEffectSpec.CanApply(Owner)) return NullEffect.Instance;
            
            inEffectSpec.SetTarget(Owner);
            inEffectSpec.Accept(EffectAppliers);
            return inEffectSpec;
        }

        public virtual void RemoveEffect(AbstractEffect abstractEffect)
        {
            for (int i = AppliedEffects.Count - 1; i >= 0; i--)
            {
                var effect = AppliedEffects[i];
                if (abstractEffect.EffectSO == effect.EffectSpec.EffectSO)
                {
                    AppliedEffects.RemoveAt(i);
                }
            }

            if (abstractEffect.Owner == null) return;
            ForceUpdateAttributeSystemModifiers();
            
            if (abstractEffect.Owner == this) return;
            abstractEffect.Owner.EffectSystem.ForceUpdateAttributeSystemModifiers();
        }

        private void Update()
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
            _attributeSystem.UpdateAllAttributeCurrentValues();
        }

        protected virtual void UpdateAttributeSystemModifiers()
        {
            // Reset all attribute to their base values
            _attributeSystem.ResetAttributeModifiers();
            foreach (var effect in AppliedEffects)
            {
                UpdateAttributesModifiersWithEffect(effect);
            }
        }

        protected void UpdateAttributesModifiersWithEffect(EffectSpecificationContainer effectContainer)
        {
            if (effectContainer.EffectSpec.IsExpired) return;

            foreach (var effectModifierDetail in effectContainer.Modifiers)
            {
                if (!_attributeSystem.HasAttribute(effectModifierDetail.Attribute))
                {
                    _attributeSystem.AddAttributes(effectModifierDetail.Attribute);
                    _attributeSystem.MarkCacheDirty();
                }

                _attributeSystem.AddModifierToAttribute(effectModifierDetail.Modifier, effectModifierDetail.Attribute, out _,
                    effectContainer.EffectSpec.EffectSO.EffectDetails.StackingType);
            }
        }
        
        protected virtual void UpdateEffects()
        {
            foreach (EffectSpecificationContainer effect in AppliedEffects)
            {
                var effectSpec = effect.EffectSpec;
                if (!effectSpec.IsExpired)
                    effectSpec.Update(Time.deltaTime);
            }
        }

        protected virtual void RemoveExpiredEffects()
        {
            for (var i = AppliedEffects.Count - 1; i >= 0; i--)
            {
                var effect = AppliedEffects[i];
                if (effect.EffectSpec.IsExpired)
                {
                    AppliedEffects.RemoveAt(i);
                    Owner.TagSystem.RemoveTags(effect.EffectSpec.EffectSO.GrantedTags);
                }
            }
        }
    }
}
