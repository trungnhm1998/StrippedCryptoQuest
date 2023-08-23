using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.TargetTypes;

namespace IndiGames.GameplayAbilitySystem.Implementation.EffectAbility
{
    [System.Serializable]
    public class AbilityEffectContainer
    {
        public TargetType TargetType;
        public EffectScriptableObject[] Effects = new EffectScriptableObject[0];
    }

    public class AbilityEffectContainerSpec
    {
        public List<GameplayEffectSpec> EffectSpecs = new List<GameplayEffectSpec>();
        public List<AbilitySystemBehaviour> Targets = new List<AbilitySystemBehaviour>();

        public void AddTargets(List<AbilitySystemBehaviour> targets)
        {
            if (targets.Count == 0) return;
            Targets.AddRange(targets);
        }
    }
}