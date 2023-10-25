using System;
using CryptoQuest.AbilitySystem.Abilities;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class NormalAttack : CharacterComponentBase
    {
        public event Action Attacked;
        [SerializeField] private NormalAttackAbility _normalAttackAbility;
        private NormalAttackAbilitySpec _spec;

        public override void Init()
        {
            _spec = Character.AbilitySystem.GiveAbility<NormalAttackAbilitySpec>(_normalAttackAbility);
        }

        public void Attack()
        {
            var target = GetComponent<ITargeting>().Target;
            OnPreAttack(target);
            Debug.Log($"{Character.DisplayName} attacking {target.DisplayName}");
            _spec.Execute(target.GetComponent<IDamageable>());
            Attacked?.Invoke();
        }

        /// <summary>
        /// Log, play vfx, share ui, etc
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected virtual void OnPreAttack(Character target) { }
    }
}