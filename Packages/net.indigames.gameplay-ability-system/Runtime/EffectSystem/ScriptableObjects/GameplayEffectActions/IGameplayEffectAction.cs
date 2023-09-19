using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions
{
    public interface IGameplayEffectAction
    {
        public ActiveEffectSpecification CreateActiveEffect(GameplayEffectSpec inSpec, AbilitySystemBehaviour owner);
    }
}