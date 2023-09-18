using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.EffectApplier
{
    /// <summary>
    /// To Execute Effect Specification based on GameplayEffect.cpp::PredictivelyExecuteEffectSpec
    /// </summary>
    public interface IEffectApplier
    {
        public ActiveEffectSpecification Visit(InstantEffectSpec instantEffectSpec);
        public ActiveEffectSpecification Visit(DurationalEffectSpec durationalEffectSpec);
        public ActiveEffectSpecification Visit(InfiniteEffectSpec infiniteEffectSpec);
    }
}