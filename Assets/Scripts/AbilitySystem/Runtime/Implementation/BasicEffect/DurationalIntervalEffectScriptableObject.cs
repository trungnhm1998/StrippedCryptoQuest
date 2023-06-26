using UnityEngine;

namespace Indigames.AbilitySystem
{
    [CreateAssetMenu(fileName = "DurationalIntevalEffect", menuName = "Indigames Ability System/Effects/Durational Effect")]
    public class DurationalIntervalEffectScriptableObject : DurationalEffectScriptableObject
    {
        public float Interval;

        protected override AbstractEffect CreateEffect()
        {
            return new DurationalIntervalEffect(Duration, Interval);
        }
    }

    /// <summary>
    /// Effect such as regen 1hp for 15 seconds every 3 seconds
    /// </summary>
    public class DurationalIntervalEffect : DurationalEffect
    {
        private readonly float _interval;
        private float _timer;

        public DurationalIntervalEffect(float duration, float interval)
        {
            _remainingDuration = duration;
            _interval = _timer = interval;
        }

        public override void Accept(IEffectApplier effectApplier)
        {
            _effectApplier = effectApplier;
            var effectContainer = new EffectSpecificationContainer(this, false);
            effectContainer.ClearModifiers();
            Target.EffectSystem.AppliedEffects.Add(effectContainer);
        }

        public override void Update(float deltaTime)
        {
            if (IsExpired) return;
            base.Update(deltaTime);

            if (_timer <= 0)
            {
                _effectApplier.ApplyInstantEffect(this);
                _timer = _interval;
            }

            _timer -= deltaTime;
        }
    }
}