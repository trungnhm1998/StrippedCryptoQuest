using System.Collections;
using CryptoQuest.Character.Ability;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.Battle.Components
{
    public class NormalAttack : CharacterComponentBase
    {
        [SerializeField] private NormalAttackAbility _normalAttackAbility;
        private NormalAttackAbilitySpec _spec;

        public override void Init()
        {
            _spec = Character.AbilitySystem.GiveAbility<NormalAttackAbilitySpec>(_normalAttackAbility);
        }

        public IEnumerator Attack()
        {
            var target = GetComponent<ITargeting>().Target;
            yield return OnPreAttack(target);
            Debug.Log($"{Character.DisplayName} attacking {target.DisplayName}");
            _spec.Execute(target.GetComponent<IDamageable>());
        }

        /// <summary>
        /// Log, play vfx, share ui, etc
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected virtual IEnumerator OnPreAttack(Character target)
        {
            yield break;
        }
    }
}