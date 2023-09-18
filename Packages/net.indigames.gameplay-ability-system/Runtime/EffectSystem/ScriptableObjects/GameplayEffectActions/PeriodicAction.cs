using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions
{
    public class PeriodicAction : GameplayEffectActionBase
    {
        public override ActiveEffectSpecification CreateActiveEffect(
            GameplayEffectSpec inSpec,
            AbilitySystemBehaviour owner) => new(inSpec);
    }
}