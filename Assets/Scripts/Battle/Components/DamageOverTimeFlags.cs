using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// This class contains DoT tags that prevent character from taking multiple DoT with the same type
    /// </summary>
    public class DamageOverTimeFlags : CharacterComponentBase
    {
        private readonly Dictionary<TagScriptableObject, bool> _damageOverTimeTags = new();
        private TinyMessageSubscriptionToken _roundEndedEvent;

        private void Awake()
        {
            Character.TurnEnded += ClearFlags;
        }

        private void OnDestroy()
        {
            Character.TurnEnded -= ClearFlags;
        }

        private void ClearFlags()
        {
            _damageOverTimeTags.Clear();
        }

        public void RaiseFlag(TagScriptableObject tag)
        {
            _damageOverTimeTags.TryAdd(tag, true);
            _damageOverTimeTags[tag] = true;
            Debug.Log("Raise flag for " + tag.name);
        }

        public bool FlagRaised(TagScriptableObject tag) => _damageOverTimeTags.ContainsKey(tag);
    }
}