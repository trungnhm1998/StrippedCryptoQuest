using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    [CreateAssetMenu(fileName = "SelfTargetSO", menuName = "Indigames Ability System/Skills/Target Type/Self Target")]
    public abstract class SelfTargetSO : TargetType
    {
        public override void GetTargets(AbilitySystem owner, ref List<AbilitySystem> targets)
        {
            targets.Add(owner);
        }
    }

}