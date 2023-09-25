using CryptoQuest.Character.Attributes;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;

namespace CryptoQuest.Battle.Components
{
    public interface ICharacter
    {
        #region Systems

        public AttributeSystemBehaviour AttributeSystem { get; }
        public EffectSystemBehaviour GameplayEffectSystem { get; }
        public AbilitySystemBehaviour AbilitySystem { get; }

        #endregion

        public Elemental Element { get; }

        public void Init(Elemental element);
    }
}