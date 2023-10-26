﻿using System.Collections.Generic;
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

        public override void Init() { } // This component got added at runtime so Init will not called

        protected override void Awake()
        {
            base.Awake();
            Character.TurnEnded += ClearFlags;
        }

        protected override void OnReset() => Character.TurnEnded -= ClearFlags;

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