using UnityEngine;
using System.Collections.Generic;

namespace Indigames.AbilitySystem
{
    public class TagSystemBehaviour : MonoBehaviour
    {
        public List<TagScriptableObject> GrantedTags = new List<TagScriptableObject>();

        public virtual void AddTags(TagScriptableObject[] tags)
        {
            GrantedTags.AddRange(tags);
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