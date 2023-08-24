using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.EffectApplier
{
    /// <summary>
    /// To Execute Effect Specification based on GameplayEffect.cpp::PredictivelyExecuteEffectSpec
    /// </summary>
    public interface IEffectApplier
    {
        public ActiveEffectSpecification Visit(IInstantEffectSpec instantEffectSpec);
        public ActiveEffectSpecification Visit(IDurationalEffectSpec durationalEffectSpec);
        public ActiveEffectSpecification Visit(IInfiniteEffectSpec infiniteEffectSpec);
    }
}