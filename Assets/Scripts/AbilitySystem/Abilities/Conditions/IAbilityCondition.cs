using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;

namespace CryptoQuest.AbilitySystem.Abilities.Conditions
{
    public interface IAbilityCondition
    {
        bool IsPass(AbilityConditionContext ctx);
    }

    public class AbilityConditionContext
    {
        public AbilitySystemBehaviour System { get; private set; }
        public GameplayEffectDefinition EffectDef { get; private set; }
        public AbilityConditionContext(AbilitySystemBehaviour system, GameplayEffectDefinition effectDef)
        {
            System = system;
            EffectDef = effectDef;
        }
    }

    [Serializable]
    public class AlwaysTrue : IAbilityCondition
    {
        public bool IsPass(AbilityConditionContext ctx) => true;
    }
}