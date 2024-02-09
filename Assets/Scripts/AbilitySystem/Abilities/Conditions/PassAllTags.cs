using System;
using IndiGames.GameplayAbilitySystem.Helper;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities.Conditions
{
    /// <summary>
    /// Return true if the system has all one tag in the accept list
    /// OR the system don't have any tag in the deny list
    /// For example: ability heal need system dont have neither FullHP nor Dead tag
    /// </summary>
    [Serializable]
    public class PassAllTags : IAbilityCondition 
    {
        [SerializeField] private TagScriptableObject[] _acceptTags;
        [SerializeField] private TagScriptableObject[] _denyTags;

        public bool IsPass(AbilityConditionContext ctx)
        {
            var tagSystem = ctx.System.TagSystem;

            foreach (var tag in _acceptTags)
            {
                if (!tagSystem.GrantedTags.CheckSystemHasTags(tag)) return false;
            }    

            foreach (var tag in _denyTags)
            {
                if (tagSystem.GrantedTags.CheckSystemHasTags(tag)) return false;
            }    
        
            return true;
        }
    }
}