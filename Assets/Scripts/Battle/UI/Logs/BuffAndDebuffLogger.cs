using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.AbilitySystem;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Character;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using CoreAttribute = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.Battle.UI.Logs
{
    /// <summary>
    /// Log buff and debuff
    /// </summary>
    public class BuffAndDebuffLogger : LoggerComponentBase
    {
        [Serializable]
        private struct TagAttributeMapping
        {
            public TagScriptableObject Tag;
            public AttributeScriptableObject Attribute;
        }

        [SerializeField] private AttributeChangeEvent _characterAttributeChangeEvent;
        [SerializeField] private LocalizedString _buffMessage;
        [SerializeField] private LocalizedString _debuffMessage;
        [SerializeField] private LocalizedString _buffWearOffMessage;
        [SerializeField] private LocalizedString _debuffWearOffMessage;
        [SerializeField] private TagAttributeMapping[] _tagAttributeMappings;
        [SerializeField] private CoreAttribute[] _ignoredAttributes;

        private TinyMessageSubscriptionToken _effectAffectingMessage;
        private TinyMessageSubscriptionToken _effectAdded;
        private TinyMessageSubscriptionToken _effectRemoved;
        private readonly Dictionary<TagScriptableObject, AttributeScriptableObject> _tagAttributeMap = new();

        private void Awake()
        {
            foreach (var tagAttributeMapping in _tagAttributeMappings)
                _tagAttributeMap.Add(tagAttributeMapping.Tag, tagAttributeMapping.Attribute);
        }

        private void OnEnable()
        {
            _characterAttributeChangeEvent.Changed += LogAttributeChange;
            _effectRemoved = BattleEventBus.SubscribeEvent<EffectRemovedEvent>(LogOnBuffRemoved);
        }

        private void OnDisable()
        {
            _characterAttributeChangeEvent.Changed -= LogAttributeChange;
            BattleEventBus.UnsubscribeEvent(_effectRemoved);
        }

        private void LogAttributeChange(AttributeSystemBehaviour owner, AttributeValue oldVal,
            AttributeValue newVal)
        {
            if (_ignoredAttributes.Contains(newVal.Attribute)) return;
            if (Mathf.Approximately(oldVal.CurrentValue, newVal.CurrentValue)) return;
            var isIncrease = oldVal.CurrentValue < newVal.CurrentValue;
            var message = isIncrease ? _buffMessage : _debuffMessage;
            if (owner.TryGetComponent(out Components.Character character) == false) return;
            message.Add(Constants.CHARACTER_NAME, new StringVariable()
            {
                Value = character.DisplayName
            });

            message.Add(Constants.ATTRIBUTE_NAME, new StringVariable()
            {
                Value = newVal.Attribute.name
            });

            Logger.AppendLog(message);
        }

        private void LogOnBuffAdded(EffectAddedEvent ctx)
        {
            if (IsBuffOrDebuff(ctx, out var tag, out var isBuff)) return;

            var message = isBuff ? _buffMessage : _debuffMessage;
            LogMessage(ctx, message, tag);
        }

        private void LogOnBuffRemoved(EffectRemovedEvent ctx)
        {
            if (IsBuffOrDebuff(ctx, out var tag, out var isBuff)) return;

            var message = isBuff ? _buffWearOffMessage : _debuffWearOffMessage;
            LogMessage(ctx, message, tag);
        }

        private void LogMessage(EffectEvent ctx, LocalizedString message, TagScriptableObject tag)
        {
            message.Add(Constants.CHARACTER_NAME, new StringVariable()
            {
                Value = ctx.Character.DisplayName
            });
            var tagMap = _tagAttributeMap[tag];
            if (tagMap.DisplayName.IsEmpty)
            {
                message.Add(Constants.ATTRIBUTE_NAME, new StringVariable()
                {
                    Value = tagMap.name
                });
            }
            else
            {
                message.Add(Constants.ATTRIBUTE_NAME, tagMap.DisplayName);
            }

            Logger.AppendLog(message);
        }

        private static bool IsBuffOrDebuff(EffectEvent ctx, out TagScriptableObject tag, out bool isBuff)
        {
            tag = ctx.Tag;
            isBuff = tag.IsChildOf(TagsDef.Buff);
            var isDebuff = tag.IsChildOf(TagsDef.DeBuff);
            return isBuff == false && isDebuff == false;
        }
    }
}