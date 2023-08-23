using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.EffectApplier
{
    /// <summary>
    /// To Execute Effect Specification based on GameplayEffect.cpp::PredictivelyExecuteEffectSpec
    /// </summary>
    public interface IEffectApplier
    {
        public void Visit(InstantEffectSpec instantEffectSpec);
        public void Visit(DurationalEffectSpec durationalEffectSpec);
        public void Visit(InfiniteEffectSpec infiniteEffectSpec);
    }
}