using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Indigames.AbilitySystem
{
    public class EffectSkill : AbstractSkill
    {
        public class EffectSkillContext
        {
            public List<SkillSystem> AffectTargets = new List<SkillSystem>();
        }

        private Dictionary<TagScriptableObject, List<AbstractEffect>> _effectTagDict =
            new Dictionary<TagScriptableObject, List<AbstractEffect>>();

        protected new EffectSkillSO SkillSO => (EffectSkillSO) _skillSO;

        protected override IEnumerator InternalActiveSkill()
        {
            // Try active skill with tag OnActive
            foreach (var effectContainer in SkillSO.EffectContainerMap)
            {
                if (effectContainer.Tag == null) continue;
                if (effectContainer.Tag.name == "OnActive")
                {
                    ApplyEffectContainerByTag(effectContainer.Tag, new EffectSkillContext());
                }
            }

            Debug.Log($"EffectSkill::InternalActiveSkill {SkillSO.name} Activated");
            yield break;
        }

        protected List<AbstractEffect> ApplyEffectContainerByTag(TagScriptableObject tag, EffectSkillContext context)
        {
            if (!IsActive)
            {
                Debug.Log($"EffectSkill::ApplyEffectContainerByTag: {SkillSO.name} is not active");
                return new List<AbstractEffect>();
            }

            var specs = CreateEffectContainerSpec(tag, context);
            Debug.Log($"EffectSkill::ApplyEffectWithTags - {tag.name}");

            var returnEffects = new List<AbstractEffect>();
            foreach (var spec in specs)
            {
                returnEffects.AddRange(ApplyEffectContainerSpec(spec, context));
            }

            if (!_effectTagDict.ContainsKey(tag))
                _effectTagDict.Add(tag, new List<AbstractEffect>());
            _effectTagDict[tag].AddRange(returnEffects);

            return returnEffects;
        }

        protected SkillEffectContainerSpec CreateEffectContainerSpecFromContainer(SkillEffectContainer container,
            EffectSkillContext context)
        {
            var returnSpec = new SkillEffectContainerSpec();
            if (Owner == null) return returnSpec;

            var targets = new List<SkillSystem>();
            if (container.TargetType)
            {
                container.TargetType.GetTargets(Owner, ref targets);
            }
            else
            {
                targets.AddRange(context.AffectTargets);
            }
            returnSpec.AddTargets(targets);

            var effects = container.Effects;
            if (effects == null) return returnSpec;

            foreach (var effect in effects)
            {
                if (effect == null) continue;
                var effectSpec = CreateEffectSpec(effect);
                returnSpec.EffectSpecs.Add(effectSpec);
            }

            return returnSpec;
        }

        protected List<SkillEffectContainerSpec> CreateEffectContainerSpec(TagScriptableObject tag,
            EffectSkillContext context)
        {
            List<SkillEffectContainerSpec> returnSpecs = new List<SkillEffectContainerSpec>();
            foreach (var effectContainer in SkillSO.EffectContainerMap)
            {
                if (effectContainer.Tag == null) continue;
                if (effectContainer.Tag == tag)
                {
                    foreach (var container in effectContainer.TargetContainer)
                    {
                        returnSpecs.Add(CreateEffectContainerSpecFromContainer(container, context));
                    }
                }
            }

            return returnSpecs;
        }

        protected virtual List<AbstractEffect> ApplyEffectContainerSpec(SkillEffectContainerSpec skillEffectSpec,
            EffectSkillContext context)
        {
            var appliedEffect = new List<AbstractEffect>();

            foreach (var effectSpec in skillEffectSpec.EffectSpecs)
            {
                foreach (var target in skillEffectSpec.Targets)
                {
                    appliedEffect.AddRange(SkillSystemHelper.ApplyEffectSpecToTarget(effectSpec, target));
                }
            }

            return appliedEffect;
        }

        public override void OnSkillRemoved(AbstractSkill skillSpec)
        {
            base.OnSkillRemoved(skillSpec);
            foreach (var tagEffect in _effectTagDict)
            {
                var effectSpecs = tagEffect.Value;
                for (int i = 0; i < effectSpecs.Count; i++)
                {
                    var effectSpec = effectSpecs[i];
                    if (effectSpec.RemoveWithSkill)
                        Owner.RemoveEffect(effectSpec);
                }
            }

            _effectTagDict.Clear();
        }

        public void RemoveEffectWithTag(TagScriptableObject skillTag)
        {
            if (!_effectTagDict.ContainsKey(skillTag)) return;
            foreach (var effectSpec in _effectTagDict[skillTag])
            {
                effectSpec.Target.RemoveEffect(effectSpec);
            }

            _effectTagDict.Remove(skillTag);
        }
    }
}