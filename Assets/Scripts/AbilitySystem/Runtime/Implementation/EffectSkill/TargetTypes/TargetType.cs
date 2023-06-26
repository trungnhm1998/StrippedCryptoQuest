using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    public abstract class TargetType : ScriptableObject
    {
        /// <summary>
        /// Add the target you want the skill to affect in targets parameter
        /// </summary>
        public abstract void GetTargets(AbilitySystem owner, ref List<AbilitySystem> targets);
    }

}