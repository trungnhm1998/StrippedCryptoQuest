using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AbilitySystem.Components
{
    public class TagSystemBehaviour : MonoBehaviour
    {
        public delegate void TagEvent(params TagScriptableObject[] tag);

        public event TagEvent TagAdded;
        public event TagEvent TagRemoved;
        [field: SerializeField] public List<TagScriptableObject> DefaultTags { get; private set; } = new();
        [field: SerializeField] public List<TagScriptableObject> GrantedTags { get; private set; } = new();

        protected virtual void Awake()
        {
            GrantedTags.AddRange(DefaultTags);
        }

        public virtual void AddTags(params TagScriptableObject[] tags)
        {
            GrantedTags.AddRange(tags);
            TagAdded?.Invoke(tags);
        }

        public virtual void RemoveTags(params TagScriptableObject[] tags)
        {
            foreach (var tag in tags)
            {
                GrantedTags.Remove(tag);
                TagRemoved?.Invoke(tag);
            }
        }

        public virtual bool HasTag(TagScriptableObject tagToCheck)
        {
            return GrantedTags.Contains(tagToCheck);
        }
    }
}