using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.EffectApplier;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    [CreateAssetMenu(menuName = "Create InstantGameplayEffect", fileName = "InstantGameplayEffect", order = 0)]
    public class InstantGameplayEffect : CryptoQuestGameplayEffect<InstantGameplayEffectSpec> { }

    public class InstantGameplayEffectSpec : CryptoQuestGameplayEffectSpec, IInstantEffectSpec
    {
        public override void Update(float deltaTime)
        {
            // because basic effect has acted already in Accept method, it is not needed to update it
        }

        public override ActiveEffectSpecification Accept(IEffectApplier effectApplier)
        {
            base.Accept(effectApplier);

            IsExpired = true;
            return _effectApplier.Visit(this);
        }
    }
}