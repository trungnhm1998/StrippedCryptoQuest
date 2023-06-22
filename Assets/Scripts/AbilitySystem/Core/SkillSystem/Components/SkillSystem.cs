using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Indigames.AbilitySystem
{
    [RequireComponent(typeof(AttributeSystem))]
    public class SkillSystem : MonoBehaviour
    {
        [SerializeField] private AttributeSystem _attributeSystem;
        public AttributeSystem AttributeSystem => _attributeSystem;

        public List<TagScriptableObject> GrantedTags = new List<TagScriptableObject>();
        public List<EffectSpecificationContainer> AppliedDurationalEffects = new List<EffectSpecificationContainer>();

        protected SkillSpecificationContainer _grantedSkills;
        public SkillSpecificationContainer GrantedSkills => _grantedSkills;

        
        private IEffectApplier _effectApplier;
        protected IEffectApplier EffectApplier
        {
            get
            {
                if (_effectApplier == null)
                {
                    _effectApplier = new EffectApplier(this);
                }

                return _effectApplier;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_attributeSystem != null) return;
            _attributeSystem = GetComponent<AttributeSystem>();
        }
#endif

#region InitSkillAndEffect
        /// <summary>
        /// Add/Give/Grant skill to the system. Only skill that in the system can be active
        /// There's only 1 skill per system (no duplicate skill)
        /// </summary>
        /// <param name="skillDef"></param>
        /// <param name="skillParams"></param>
        /// <returns>A skill handler (humble object) to execute their skill logic</returns>
        public AbstractSkill GiveSkill(SkillScriptableObject skillDef, SkillParameters skillParams)
        {
            foreach (var skill in _grantedSkills.Skills)
            {
                if (skill.SkillSO == skillDef)
                    return skill;
            }

            var grantedSkill = skillDef.GetSkillSpec(this, skillParams);

            return GiveSkill(grantedSkill);
        }

        public AbstractSkill GiveSkill(AbstractSkill inSkillSpec)
        {
            if (!inSkillSpec.SkillSO) return null;

            foreach (AbstractSkill skillSpec in _grantedSkills.Skills)
            {
                if (skillSpec.SkillSO == inSkillSpec.SkillSO)
                    return skillSpec;
            }
            _grantedSkills.Skills.Add(inSkillSpec);
            OnGrantedSkill(inSkillSpec);

            return inSkillSpec;
        }

        private void OnGrantedSkill(AbstractSkill skillSpec)
        {
            if (!skillSpec.SkillSO) return;
            skillSpec.Owner = this;
            skillSpec.OnSkillGranted(skillSpec);
        }

        /// <summary>
        /// Will create a new AbstractEffect from EffectScriptableObject (data)
        /// this will update the Owner of the effect to this SkillSystem
        /// </summary>
        /// <param name="effectSO"></param>
        /// <param name="origin"></param>
        /// <param name="skillParameters"></param>
        /// <returns></returns>
        public AbstractEffect GetEffect(EffectScriptableObject effectSO, object origin, SkillParameters skillParameters)
        {
            UpdateAttributeSystemModifiers();
            return effectSO.GetEffect(this, origin, skillParameters);
        }
#endregion

#region SkillActivationAndApplyEffect
        public void TryActiveSkill(AbstractSkill inSkillSpec)
        {
            if (inSkillSpec.SkillSO == null) return;
            foreach (var skillSpec in _grantedSkills.Skills)
            {
                if (skillSpec != inSkillSpec) continue;
                inSkillSpec.ActivateSkill();
            }
        }

        public AbstractEffect ApplyEffectToSelf(AbstractEffect inEffectSpec)
        {
            if (inEffectSpec == null || !inEffectSpec.CanApply(this)) return NullEffect.Instance;
            
            inEffectSpec.SetTarget(this);
            inEffectSpec.Accept(EffectApplier);
            return inEffectSpec;
        }
#endregion

#region RemoveSkillAndEffect
        public bool RemoveSkill(AbstractSkill skill)
        {
            List<AbstractSkill> grantedSkillsList = _grantedSkills.Skills;
            for (int i = grantedSkillsList.Count - 1; i >= 0; i--)
            {
                var skillSpec = grantedSkillsList[i];
                if (skillSpec == skill)
                {
                    OnRemoveSkill(skillSpec);
                    grantedSkillsList.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }
        
        private void OnRemoveSkill(AbstractSkill skillSpec)
        {
            if (!skillSpec.SkillSO) return;

            skillSpec.OnSkillRemoved(skillSpec);
        }
        
        public void RemoveAllSkills()
        {
            for (int i = _grantedSkills.Skills.Count - 1; i >= 0; i--)
            {
                var skillSpec = _grantedSkills.Skills[i];
                _grantedSkills.Skills.RemoveAt(i);
                OnRemoveSkill(skillSpec);
            }
            _grantedSkills.Skills = new List<AbstractSkill>();
        }
        
        public virtual void RemoveEffect(AbstractEffect abstractEffect)
        {
            for (int i = AppliedDurationalEffects.Count - 1; i >= 0; i--)
            {
                var effect = AppliedDurationalEffects[i];
                if (abstractEffect.EffectSO == effect.EffectSpec.EffectSO)
                {
                    AppliedDurationalEffects.RemoveAt(i);
                }
            }

            if (abstractEffect.Owner == null) return;
            ForceUpdateAttributeSystemModifiers();
            
            if (abstractEffect.Owner == this) return;
            abstractEffect.Owner.ForceUpdateAttributeSystemModifiers();
        }
#endregion

#region Tags
        public virtual void AddTags(TagScriptableObject[] tags)
        {
            GrantedTags.AddRange(tags);
        }

        public virtual void RemoveTags(TagScriptableObject[] tags)
        {
            foreach (var tag in tags)
            {
                GrantedTags.Remove(tag);
            }
        }

        public virtual bool HasTag(TagScriptableObject tagToCheck)
        {
            return GrantedTags.Contains(tagToCheck);
        }
        
        public virtual bool CheckTagRequirementsMet(AbstractEffect effect)
        {
            var tagConditionDetail = effect.EffectSO.ApplicationTagRequirements;
            return SkillSystemHelper.SystemHasAllTags(this, tagConditionDetail.RequireTags) 
                && SkillSystemHelper.SystemHasNoneTags(this, tagConditionDetail.IgnoreTags);
        }
#endregion

        private void Update()
        {
            UpdateAttributeSystemModifiers();

            UpdateEffects();
            RemoveExpiredEffects();
            RemovePendingSkills();
        }

        /// <summary>
        /// Force the system to check all effects and update their status
        /// </summary>
        public void ForceUpdateAttributeSystemModifiers()
        {
            UpdateAttributeSystemModifiers();
            _attributeSystem.UpdateAttributeCurrentValues();
        }

        protected virtual void UpdateAttributeSystemModifiers()
        {
            // Reset all attribute to their base values
            _attributeSystem.ResetAttributeModifiers();
            foreach (var effect in AppliedDurationalEffects)
            {
                UpdateAttributesModifiersWithEffect(effect);
            }
        }

        protected void UpdateAttributesModifiersWithEffect(EffectSpecificationContainer effectContainer)
        {
            if (effectContainer.EffectSpec.IsExpired) return;

            foreach (var modifierDetail in effectContainer.Modifiers)
            {
                if (!_attributeSystem.HasAttribute(modifierDetail.Attribute))
                {
                    _attributeSystem.AddAttributes(modifierDetail.Attribute);
                    _attributeSystem.MarkCacheDirty();
                }

                _attributeSystem.AddModifierToAttribute(modifierDetail.Modifier, modifierDetail.Attribute, out _,
                    effectContainer.EffectSpec.EffectSO.EffectDetails.StackingType);
            }
        }
        
        protected virtual void UpdateEffects()
        {
            foreach (EffectSpecificationContainer effect in AppliedDurationalEffects)
            {
                var effectSpec = effect.EffectSpec;
                if (!effectSpec.IsExpired)
                    effectSpec.Update(Time.deltaTime);
            }
        }

        protected virtual void RemoveExpiredEffects()
        {
            for (var i = AppliedDurationalEffects.Count - 1; i >= 0; i--)
            {
                var effect = AppliedDurationalEffects[i];
                if (effect.EffectSpec.IsExpired)
                {
                    AppliedDurationalEffects.RemoveAt(i);
                }
            }
        }

        protected virtual void RemovePendingSkills()
        {
            foreach (var skill in _grantedSkills.Skills.ToList())
            {
                if (skill.IsPendingRemove || skill.IsRemoveAfterActivation)
                    RemoveSkill(skill);
            }
        }
    }
}
