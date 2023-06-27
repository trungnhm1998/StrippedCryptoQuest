using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    [CreateAssetMenu(fileName = "SelfTargetSO", menuName = "Indigames Ability System/Abilities/Target Type/Self Target")]
    public class SelfTargetSO : TargetType
    {
        public override void GetTargets(AbilitySystemBehaviour owner, ref List<AbilitySystemBehaviour> targets)
        {
            targets.Add(owner);
        }
    }

}