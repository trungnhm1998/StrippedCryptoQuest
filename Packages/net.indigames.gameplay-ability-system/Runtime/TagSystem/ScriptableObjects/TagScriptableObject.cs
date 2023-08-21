using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Indigames Ability System/Tag")]
    public class TagScriptableObject : ScriptableObject
    {
        [SerializeField] private TagScriptableObject _parent;
        public TagScriptableObject Parent => _parent;

        /// <summary>
        /// <para>Check if the tag is child/descendant of other tag.</para>
        /// By default will search only 3 levels deep.
        /// </summary>
        /// <param name="other">Parent/Ancestor tag to compare with</param>
        /// <param name="depthSearchLimit">depth limit to search, increase this if needed for more complex system</param>
        /// <returns>True if this tag is a child/descendant of the other tag</returns>
        public bool IsChildOf(TagScriptableObject other, int depthSearchLimit = 3)
        {
            var tag = _parent;
            var i = depthSearchLimit;
            while (i-- >=  0)
            {
                if (depthSearchLimit <= 0) return false;

                // If we have no parent, we are not a child of anything. At root
                if (tag == null) return false;

                // If the current tag is the same as the one we are checking, we are a child of it
                if (tag == other) return true;

                tag = tag.Parent;
                depthSearchLimit -= 1;
            }

            return false;
        }
    }
}