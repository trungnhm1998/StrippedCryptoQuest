using CryptoQuest.Gameplay.Battle.Core;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;

namespace CryptoQuest.Character.Ability
{
    public class GameplayEffectSO : GameplayEffectDefinition
    {
        public EffectSpec CreateEffectSpec(AbilitySystemBehaviour abilitySystem, CastSkillAbilitySpec abilitySpec)
        {
            var effect = new EffectSpec(abilitySpec);
            effect.InitEffect(this, abilitySystem);
            return effect;
        }
    }

    public class EffectSpec : GameplayEffectSpec
    {
        private readonly CastSkillAbilitySpec _abilitySpec;
        public CastSkillAbilitySpec AbilitySpec => _abilitySpec;
        public SkillParameters Parameters => _abilitySpec.Def.Parameters.SkillParameters;

        public EffectSpec(CastSkillAbilitySpec abilitySpec)
        {
            _abilitySpec = abilitySpec;
        }
    }
}