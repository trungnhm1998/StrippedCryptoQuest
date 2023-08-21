using System;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects
{
    [Serializable]
    public class TagRequireIgnoreDetails
    {
        [Tooltip("All of these tags must be present in the ability system")]
        public TagScriptableObject[] RequireTags = new TagScriptableObject[0];
        
        [Tooltip("None of these tags can be present in the ability system")]
        public TagScriptableObject[] IgnoreTags = new TagScriptableObject[0];
    }
}