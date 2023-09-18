using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AbilitySystem.Components
{
    public class TagSystemBehaviour : MonoBehaviour
    {
        [field: SerializeField] public List<TagScriptableObject> DefaultTags { get; private set; } = new();
        [field: SerializeField] public List<TagScriptableObject> GrantedTags { get; private set; } = new();

        private void Awake()
        {
            GrantedTags.AddRange(DefaultTags);
        }

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