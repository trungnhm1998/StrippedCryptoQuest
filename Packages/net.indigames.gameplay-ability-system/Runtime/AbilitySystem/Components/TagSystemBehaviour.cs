using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AbilitySystem.Components
{
    public class TagSystemBehaviour : MonoBehaviour
    {
        public List<TagScriptableObject> GrantedTags = new List<TagScriptableObject>();

        public virtual void AddTags(TagScriptableObject[] tags)
        {
            GrantedTags.AddRange(tags);
        }

        public virtual void AddTags(TagScriptableObject tag)
        {
            GrantedTags.Add(tag);
        }

        public virtual void RemoveTags(TagScriptableObject[] tags)
        {
            foreach (var tag in tags)
            {
                GrantedTags.Remove(tag);
            }
        }

        public virtual bool HasTag(TagScriptableObject tagToCheck)
        {
            return GrantedTags.Contains(tagToCheck);
        }
    }
}