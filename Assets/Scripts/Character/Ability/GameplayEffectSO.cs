using CryptoQuest.Gameplay.Battle.Core;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;

namespace CryptoQuest.Character.Ability
{
    public class GameplayEffectSO : GameplayEffectDefinition
    {
        protected override GameplayEffectSpec CreateEffect() => new CQEffectSpec();
    }

    public class CQEffectSpec : GameplayEffectSpec
    {
        public SkillParameters Parameters { get; set; }
    }
}