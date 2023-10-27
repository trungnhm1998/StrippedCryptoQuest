using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class NormalAttack : CharacterComponentBase
    {
        public delegate void AttackDelegate(Character attacker, Character target, float damage);

        public event AttackDelegate Attacking;
        public event AttackDelegate Attacked;
        [SerializeField] private NormalAttackAbility _normalAttackAbility;
        private NormalAttackAbilitySpec _spec;

        public override void Init() =>
            _spec = Character.AbilitySystem.GiveAbility<NormalAttackAbilitySpec>(_normalAttackAbility);

        public void Attack()
        {
            var target = GetComponent<ITargeting>().Target;
            Attacking?.Invoke(Character, target, 0f);
            Debug.Log($"{Character.DisplayName} attacking {target.DisplayName}");
            target.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var health);
            _spec.Execute(target.GetComponent<IDamageable>());
            target.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var healthAfterTakingDamage);
            Attacked?.Invoke(Character, target, health.CurrentValue - healthAfterTakingDamage.CurrentValue);
        }
    }
}