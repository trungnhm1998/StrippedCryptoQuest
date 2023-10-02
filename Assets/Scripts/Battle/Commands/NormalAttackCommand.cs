using System.Collections;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Character.Tag;
using UnityEngine;

namespace CryptoQuest.Battle.Commands
{
    public class NormalAttackCommand : ICommand
    {
        private readonly NormalAttack _attacker;
        private readonly ITargeting _targetComponent;

        public NormalAttackCommand(Components.Character attacker, Components.Character target)
        {
            _attacker = attacker.GetComponent<NormalAttack>();
            _targetComponent = attacker.GetComponent<ITargeting>();
            _targetComponent.Target = target;
        }

        public IEnumerator Execute()
        {
            if (_attacker == null || _targetComponent.Target == null)
            {
                Debug.LogWarning("Failed to execute NormalAttackCommand. Attacker or target is null.");
                yield break;
            }

            _targetComponent.Target.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var hpBefore);
            Debug.Log(
                $"NormalAttackCommand::Executed::Before {hpBefore.CurrentValue} has dead tag {_targetComponent.Target.HasTag(TagsDef.Dead)}");
            _attacker.Attack();
            _targetComponent.Target.AttributeSystem.TryGetAttributeValue(AttributeSets.Health, out var hpAfter);
            Debug.Log(
                $"NormalAttackCommand::Executed::After {hpAfter.CurrentValue} has dead tag {_targetComponent.Target.HasTag(TagsDef.Dead)}");
        }
    }
}