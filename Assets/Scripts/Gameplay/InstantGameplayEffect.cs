using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.EffectApplier;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;

namespace CryptoQuest.Gameplay
{
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