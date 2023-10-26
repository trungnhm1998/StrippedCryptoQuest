using CryptoQuest.AbilitySystem.Abilities;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class NormalAttack : CharacterComponentBase
    {
        public delegate void AttackDelegate(Character target);

        public event AttackDelegate Attacking;
        public event AttackDelegate Attacked;
        [SerializeField] private NormalAttackAbility _normalAttackAbility;
        private NormalAttackAbilitySpec _spec;

        public override void Init() =>
            _spec = Character.AbilitySystem.GiveAbility<NormalAttackAbilitySpec>(_normalAttackAbility);

        public void Attack()
        {
            var target = GetComponent<ITargeting>().Target;
            Attacking?.Invoke(target);
            Debug.Log($"{Character.DisplayName} attacking {target.DisplayName}");
            _spec.Execute(target.GetComponent<IDamageable>());
            Attacked?.Invoke(target);
        }
    }
}