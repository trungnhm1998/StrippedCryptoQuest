using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;

namespace CryptoQuest.Battle.Components
{
    public interface ICharacter
    {
        public AttributeSystemBehaviour AttributeSystem { get; }
        public EffectSystemBehaviour GameplayEffectSystem { get; }
        public AbilitySystemBehaviour AbilitySystem { get; }
        public void Init();
    }
}