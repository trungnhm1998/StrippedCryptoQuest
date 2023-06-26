using UnityEngine;

namespace Indigames.AbilitySystem
{
    [CreateAssetMenu(fileName = "InstantEffect", menuName = "Indigames Ability System/Effects/Instant Effect")]
    public class InstantEffectScriptableObject : EffectScriptableObject<InstantEffect>
    {
    }

    public class InstantEffect : AbstractEffect
    {
        public override void Update(float deltaTime)
        {
            // because basic effect has acted already in Accept method, it is not needed to update it
        }

        public override void Accept(IEffectApplier effectApplier)
        {
            base.Accept(effectApplier);

            _effectApplier.ApplyInstantEffect(this);
            IsExpired = true;
        }
    }
}