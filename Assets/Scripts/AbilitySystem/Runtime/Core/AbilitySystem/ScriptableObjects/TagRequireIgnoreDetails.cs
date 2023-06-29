using System;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    [Serializable]
    public struct TagRequireIgnoreDetails
    {
        [Tooltip("All of these tags must be present in the ability system")]
        public TagScriptableObject[] RequireTags;
        
        [Tooltip("None of these tags can be present in the ability system")]
        public TagScriptableObject[] IgnoreTags;
    }
}