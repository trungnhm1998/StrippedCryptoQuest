using System;
using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class AddEffectLogger : MonoBehaviour
    {
        [Serializable]
        private struct TagAttributeMapping
        {
            public TagScriptableObject Tag;
            public AttributeScriptableObject Attribute;
        }

        [SerializeField] private UnityEvent<LocalizedString> _presentLoggerEvent;

        [SerializeField] private LocalizedString _messageLog;
        [SerializeField] private TagAttributeMapping[] _tagAttributeMappings;
        [SerializeField] private AttributeConfigMapping _attributeConfigMapping;
        private TinyMessageSubscriptionToken _effectAdded;
        private readonly Dictionary<TagScriptableObject, AttributeScriptableObject> _tagAttributeMap = new();

        private void Awake()
        {
            foreach (var tagAttributeMapping in _tagAttributeMappings)
                _tagAttributeMap.Add(tagAttributeMapping.Tag, tagAttributeMapping.Attribute);
        }

        private void OnEnable()
        {
            _effectAdded = BattleEventBus.SubscribeEvent<EffectAddedEvent>(LogOnEffectAdded);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_effectAdded);
        }

        private void LogOnEffectAdded(EffectAddedEvent ctx)
        {
            if (!_tagAttributeMap.TryGetValue(ctx.Tag, out var attribute)) return;
            if (!_attributeConfigMapping.TryGetMap(attribute, out var map)) return;

            var log = new LocalizedString(_messageLog.TableReference, _messageLog.TableEntryReference)
            {
                { Constants.CHARACTER_NAME, ctx.Character.LocalizedName },
                { Constants.ATTRIBUTE_NAME, map.Name }
            };

            _presentLoggerEvent.Invoke(log);
        }
    }
}