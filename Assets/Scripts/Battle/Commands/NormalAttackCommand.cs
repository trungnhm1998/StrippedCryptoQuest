using CryptoQuest.Battle.Components;
using UnityEngine;

namespace CryptoQuest.Battle.Commands
{
    public class NormalAttackCommand : ICommand
    {
        private readonly NormalAttack _attacker;
        private readonly IDamageable _target;

        public NormalAttackCommand(GameObject attacker, GameObject target)
        {
            _attacker = attacker.GetComponent<NormalAttack>();
            _target = target.GetComponent<IDamageable>();
        }

        public void Execute()
        {
            if (_attacker == null || _target == null)
            {
                Debug.LogWarning("Failed to execute NormalAttackCommand. Attacker or target is null.");
                return;
            }

            _attacker.Attack(_target);
        }
    }
}