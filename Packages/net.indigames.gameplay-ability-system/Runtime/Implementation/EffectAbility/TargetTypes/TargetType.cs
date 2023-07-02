using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.TargetTypes
{
    public abstract class TargetType : ScriptableObject
    {
        /// <summary>
        /// Add the target you want the ability to affect in targets parameter
        /// </summary>
        public abstract void GetTargets(AbilitySystemBehaviour owner, ref List<AbilitySystemBehaviour> targets);
    }

}