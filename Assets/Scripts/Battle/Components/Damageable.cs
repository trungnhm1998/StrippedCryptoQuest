using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;

namespace CryptoQuest.Battle.Components
{
    public interface IDamageable
    {
        public ActiveGameplayEffect ReceiveDamage(GameplayEffectSpec damageSpec);
    }

    public class Damageable : CharacterComponentBase, IDamageable
    {
        private AbilitySystemBehaviour _abilitySystem;

        protected override void OnInit() => _abilitySystem = Character.AbilitySystem;

        public ActiveGameplayEffect ReceiveDamage(GameplayEffectSpec damageSpec) => _abilitySystem.ApplyEffectSpecToSelf(damageSpec);
    }
}