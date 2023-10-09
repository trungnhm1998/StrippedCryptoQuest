using System.Collections;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Character.Tag;
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

        public IEnumerator Execute()
        {
            if (_attacker == null || _targetComponent.Target == null)
            {
                Debug.LogWarning("Failed to execute NormalAttackCommand. Attacker or target is null.");
                yield break;
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
                yield break;
            }

            _targetComponent.Target.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var hpBefore);
            Debug.Log(
                $"NormalAttackCommand::Executed::Before {hpBefore.CurrentValue} has dead tag {_targetComponent.Target.HasTag(TagsDef.Dead)}");
            yield return _attacker.Attack();
            _targetComponent.Target.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var hpAfter);
            Debug.Log(
                $"NormalAttackCommand::Executed::After {hpAfter.CurrentValue} has dead tag {_targetComponent.Target.HasTag(TagsDef.Dead)}");
        }

        private bool IsTargetEvaded()
        {
            var evadeBehaviour = _targetComponent.Target.GetComponent<IEvadable>();
            return evadeBehaviour != null && evadeBehaviour.TryEvade();
        }
    }
}