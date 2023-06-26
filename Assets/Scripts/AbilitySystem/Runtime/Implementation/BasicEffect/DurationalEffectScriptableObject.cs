using UnityEngine;

namespace Indigames.AbilitySystem
{
    [CreateAssetMenu(fileName = "DurationalEffect", menuName = "Indigames Ability System/Effects/Durational Effect")]
    public class DurationalEffectScriptableObject : EffectScriptableObject<DurationalEffect>
    {
        public float Duration;
    }

    /// <summary>
    /// Effect need update over time such as slow or stun enemy for 3 seconds
    /// </summary>
    public class DurationalEffect : AbstractEffect
    {
        protected float _remainingDuration;

        public float RemainingDuration
        {
            get => _remainingDuration;
            set => _remainingDuration = value;
        }

        public override void InitEffect(EffectScriptableObject effectScriptableObject, AbilitySystem ownerSystem,
            AbilityParameters parameters)
        {
            base.InitEffect(effectScriptableObject, ownerSystem, parameters);
            _remainingDuration = ((DurationalEffectScriptableObject) effectScriptableObject).Duration;
        }

        public override void Accept(IEffectApplier effectApplier)
        {
            base.Accept(effectApplier);
            _effectApplier.ApplyDurationalEffect(this);
        }

        public override void Update(float deltaTime)
        {
            _remainingDuration -= deltaTime;
            if (_remainingDuration <= 0)
            {
                IsExpired = true;
            }
        }
    }
}