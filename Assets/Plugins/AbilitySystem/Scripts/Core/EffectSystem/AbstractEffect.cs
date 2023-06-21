using System;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    [Serializable]
    public abstract class AbstractEffect
    {
        /// <summary>
        /// Which Data/SO the effect is based on
        /// </summary>
        public EffectScriptableObject EffectSO { get; private set; }

        private SkillSystem _target;
        public SkillSystem Target => _target;
        public SkillSystem Owner;
        public SkillParameters SkillParameters;
        public int Level { get; set; } = 1;

        /// <summary>
        /// Add level rate to modifier value
        /// </summary>
        public float LevelRate { get; set; } = 0;

        public bool IsExpired { get; set; } = false;
        public bool IsRemoveAfterApplied
        {
            get
            {
                if (EffectSO == null)
                    return false;
                return EffectSO.RemoveAfterApplied;
            }
        }

        // TODO: Move out of this abstract class
        public float Value;

        protected IEffectApplier _effectApplier;

        public bool RemoveWithSkill = true;

        // TODO: We will need to know the "origin"
        public string Origin;

        public virtual void InitEffect(EffectScriptableObject effectScriptableObject, SkillSystem skillSystem,
            SkillParameters parameters)
        {
            Owner = skillSystem;
            SkillParameters = parameters;

            if (effectScriptableObject == null)
            {
                Debug.LogWarning("EffectSO is null");
                return;
            }

            EffectSO = effectScriptableObject;
        }

        public bool CanApply(SkillSystem skillSystem)
        {
            if (Owner == null) return false;
            if (!skillSystem.CheckTagRequirementsMet(this)) return false;

            var effectSODetails = EffectSO.EffectDetails;

            foreach (var modifier in effectSODetails.Modifiers)
            {
                if (!modifier.AttributeSO)
                    return false;
            }

            if (UnityEngine.Random.value > EffectSO.ChanceToApply)
                return false;

            foreach (var appReq in EffectSO.CustomApplicationRequirements)
            {
                if (appReq != null && !appReq.CanApplyGameplayEffect(EffectSO, this, skillSystem))
                    return false;
            }

            //TODO: Stackable durational effect condition might inherit this and make a custom effect for stacking effect
            return true;
        }

        public AbstractEffect SetTarget(SkillSystem skillSystem)
        {
            _target = skillSystem;
            return this;
        }

        public virtual void Accept(IEffectApplier effectApplier)
        {
            _effectApplier = effectApplier;
        }

        public abstract void Update(float deltaTime);
        public AbstractEffect Clone()
        {
            var clone = Owner.GetEffect(EffectSO, Origin, SkillParameters);
            clone.Value = Value;

            return clone;
        }
    }

    public class NullEffect : AbstractEffect
    {
        private static NullEffect _instance;

        public static NullEffect Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NullEffect();
                return _instance;
            }
        }

        private NullEffect()
        {
            IsExpired = true;
        }

        public override void Update(float deltaTime)
        {
        }
    }
}