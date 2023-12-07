using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;

namespace CryptoQuest.AbilitySystem.Effects
{
    public class PassiveEffectDef : GameplayEffectDefinition
    {
        protected override GameplayEffectSpec CreateEffect() => new PassiveEffectSpec();
    }

    public class PassiveEffectSpec : GameplayEffectSpec
    {
        public override int CompareTo(GameplayEffectSpec other) => ReferenceEquals(this, other) ? 1 : 0;
    }
}