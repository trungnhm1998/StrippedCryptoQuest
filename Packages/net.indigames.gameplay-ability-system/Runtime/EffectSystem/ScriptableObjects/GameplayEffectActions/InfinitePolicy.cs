using System;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions
{
    [Serializable]
    public class InfinitePolicy : IGameplayEffectPolicy
    {
        public InfinitePolicy() { }
        public ActiveGameplayEffect CreateActiveEffect(GameplayEffectSpec inSpec) => new(inSpec);
    }
}