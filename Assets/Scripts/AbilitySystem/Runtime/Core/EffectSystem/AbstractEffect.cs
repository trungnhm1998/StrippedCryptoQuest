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

        /// <summary>
        /// The system give/apply this effect
        /// </summary>
        public AbilitySystemBehaviour Owner;

        /// <summary>
        /// The system this effect apply to. Can be the same as Owner and it will be self apply effect
        /// </summary>
        private AbilitySystemBehaviour _target;
        public AbilitySystemBehaviour Target => _target;

        // The object that create this effect
        public string Origin;
        public AbilityParameters Parameters;
        public bool IsExpired { get; set; } = false;
        public bool RemoveWhenAbilityEnd = true;

        protected IEffectApplier _effectApplier;

        public virtual void InitEffect(EffectScriptableObject effectScriptableObject, AbilitySystemBehaviour ownerSystem,
            AbilityParameters parameters)
        {
            Owner = ownerSystem;
            Parameters = parameters;

            if (effectScriptableObject == null)
            {
                Debug.LogWarning("EffectSO is null");
                return;
            }

            EffectSO = effectScriptableObject;
        }

        public bool CanApply()
        {
            if (Owner == null) return false;
            if (!CheckTagRequirementsMet()) return false;

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
                if (appReq != null && !appReq.CanApplyEffect(EffectSO, this, Owner))
                    return false;
            }

            return true;
        }

        public AbstractEffect SetTarget(AbilitySystemBehaviour targetSystem)
        {
            _target = targetSystem;
            return this;
        }

        public virtual void Accept(IEffectApplier effectApplier)
        {
            _effectApplier = effectApplier;
        }

        public abstract void Update(float deltaTime);
        public AbstractEffect Clone()
        {
            var clone = Owner.EffectSystem.GetEffect(EffectSO, Origin, Parameters);
            return clone;
        }

        protected virtual bool CheckTagRequirementsMet()
        {
            var tagConditionDetail = EffectSO.ApplicationTagRequirements;
            return AbilitySystemHelper.SystemHasAllTags(Owner, tagConditionDetail.RequireTags) 
                && AbilitySystemHelper.SystemHasNoneTags(Owner, tagConditionDetail.IgnoreTags);
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