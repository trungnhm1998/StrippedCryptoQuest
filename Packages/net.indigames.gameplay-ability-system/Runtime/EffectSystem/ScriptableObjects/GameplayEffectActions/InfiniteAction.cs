using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions
{
    public class InfiniteAction : IGameplayEffectAction
    {
        public InfiniteAction() { }

        public ActiveEffectSpecification CreateActiveEffect(
            GameplayEffectSpec inSpec,
            AbilitySystemBehaviour owner) => new(inSpec);
    }
}