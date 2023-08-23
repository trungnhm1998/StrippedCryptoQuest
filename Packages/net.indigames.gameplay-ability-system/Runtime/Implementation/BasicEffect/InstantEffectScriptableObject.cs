using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.EffectApplier;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.Implementation.BasicEffect
{
    [CreateAssetMenu(fileName = "InstantEffect", menuName = "Indigames Ability System/Effects/Instant Effect")]
    public class InstantEffectScriptableObject : EffectScriptableObject<InstantEffectSpec>
    {
    }

    public class InstantEffectSpec : GameplayEffectSpec
    {
        public override void Update(float deltaTime)
        {
            // because basic effect has acted already in Accept method, it is not needed to update it
        }

        public override void Accept(IEffectApplier effectApplier)
        {
            base.Accept(effectApplier);

            _effectApplier.Visit(this);
            IsExpired = true;
        }
    }
}