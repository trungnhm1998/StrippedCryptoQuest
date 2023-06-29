using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    public abstract class TargetType : ScriptableObject
    {
        /// <summary>
        /// Add the target you want the ability to affect in targets parameter
        /// </summary>
        public abstract void GetTargets(AbilitySystemBehaviour owner, ref List<AbilitySystemBehaviour> targets);
    }

}