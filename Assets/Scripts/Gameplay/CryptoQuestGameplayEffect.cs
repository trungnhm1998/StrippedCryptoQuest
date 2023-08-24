using CryptoQuest.Gameplay.Battle.Core;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;

namespace CryptoQuest.Gameplay
{
    public abstract class CryptoQuestGameplayEffect : EffectScriptableObject<CryptoQuestGameplayEffectSpec>
    {
    }

    public abstract class CryptoQuestGameplayEffect<TSpec> : CryptoQuestGameplayEffect
        where TSpec : CryptoQuestGameplayEffectSpec, new()
    {
        protected override GameplayEffectSpec CreateEffect() => new TSpec();
    }

    public class CryptoQuestGameplayEffectSpec : GameplayEffectSpec
    {
        public SkillParameters SkillParam { get; private set; }

        public void SetParameters(SkillParameters infoSkillParameters)
        {
            SkillParam = infoSkillParameters;
        }
    }
}