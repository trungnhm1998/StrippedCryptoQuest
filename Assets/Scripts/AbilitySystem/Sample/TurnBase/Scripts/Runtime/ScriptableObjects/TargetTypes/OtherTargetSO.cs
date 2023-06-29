using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    [CreateAssetMenu(fileName = "SelfTargetSO", menuName = "Indigames Ability System/Abilities/Target Type/Other Target")]
    public class OtherTargetSO : TargetType
    {
        [SerializeField] private TargetContainterSO _targetContainer;
        public override void GetTargets(AbilitySystemBehaviour owner, ref List<AbilitySystemBehaviour> targets)
        {
            targets.AddRange(_targetContainer.Targets);
        }
    }

}