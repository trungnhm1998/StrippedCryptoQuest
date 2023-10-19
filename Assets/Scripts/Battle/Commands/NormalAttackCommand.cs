using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using UnityEngine;

namespace CryptoQuest.Battle.Commands
{
    public class NormalAttackCommand : ICommand
    {
        private readonly NormalAttack _attacker;
        private readonly ITargeting _targetComponent;
        private readonly Components.Character _attackerCharacter;
        private readonly Components.Character _targetCharacter;

        public NormalAttackCommand(Components.Character attacker, Components.Character targetCharacter)
        {
            _targetCharacter = targetCharacter;
            _attackerCharacter = attacker;
            _attacker = attacker.GetComponent<NormalAttack>();
            _targetComponent = attacker.GetComponent<ITargeting>();
            _targetComponent.Target = targetCharacter;
        }

        public void Execute()
        {
            if (_attacker == null || _targetComponent.Target == null)
            {
                Debug.LogWarning("Failed to execute NormalAttackCommand. Attacker or target is null.");
                return;
            }

            BattleEventBus.RaiseEvent(new NormalAttackEvent()
            {
                Character = _attackerCharacter,
                Target = _targetCharacter
            });

            if (IsTargetEvaded())
            {
                BattleEventBus.RaiseEvent(new MissedEvent()
                {
                    Character = _targetCharacter
                });
            }

            _attacker.Attack();
        }

        private bool IsTargetEvaded()
        {
            var evadeBehaviour = _targetComponent.Target.GetComponent<IEvadable>();
            return evadeBehaviour != null && evadeBehaviour.TryEvade();
        }
    }
}