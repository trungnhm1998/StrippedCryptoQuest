using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EffectAbility", menuName = "Indigames Ability System/Abilities/Effect Ability")]
    public class EffectAbilitySO : AbilityScriptableObject<EffectAbility>
    {
        /// <summary>
        /// Effect applied using tag in desired timing (eg. PostAttack,...)
        /// Or assign a Tag named OnActive to apply the effect when activate ability
        /// </summary>
        /// <typeparam name="EffectTagMap"></typeparam>
        /// <returns></returns>
        public List<EffectTagMap> EffectContainerMap = new();
        
        [Serializable]
        public class EffectTagMap
        {
            public TagScriptableObject Tag;
            public List<AbilityEffectContainer> TargetContainer = new List<AbilityEffectContainer>();
        }
    }
}