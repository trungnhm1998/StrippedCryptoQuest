using CryptoQuest.Gameplay;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;

namespace CryptoQuest.Battle
{
    public interface ICharacter
    {
        public AttributeSystemBehaviour Attributes { get; }
        public EffectSystemBehaviour GameplayEffectSystem { get; }
        public AbilitySystemBehaviour AbilitySystem { get; }
        public Elemental Element { get; set; }
        public void Init();
        void Attack(ICharacter enemy);
    }
}