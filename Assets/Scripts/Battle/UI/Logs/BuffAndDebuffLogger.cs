using System;
using System.Collections.Generic;
using CryptoQuest.AbilitySystem;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

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

        [SerializeField] private StatChangedLogger _statChangedLogger;

        [SerializeField] private LocalizedString _applyFailMessage;
        [SerializeField] private LocalizedString _buffWearOffMessage;
        [SerializeField] private LocalizedString _debuffWearOffMessage;
        [SerializeField] private TagAttributeMapping[] _tagAttributeMappings;

        private TinyMessageSubscriptionToken _effectAdded;
        private TinyMessageSubscriptionToken _effectRemoved;
        private readonly Dictionary<TagScriptableObject, AttributeScriptableObject> _tagAttributeMap = new();

        private bool _isJustAddEffect;
        private Components.Character _lastCharacterGotEffect;

        private void Awake()
        {
            foreach (var tagAttributeMapping in _tagAttributeMappings)
                _tagAttributeMap.Add(tagAttributeMapping.Tag, tagAttributeMapping.Attribute);
        }

        private void OnEnable()
        {
            _effectRemoved = BattleEventBus.SubscribeEvent<EffectRemovedEvent>(LogOnBuffRemoved);
            _effectAdded = BattleEventBus.SubscribeEvent<EffectAddedEvent>(LogOnBuffAdded);
            _statChangedLogger.StatsChangedWithSameValue += LogApplyFail;
            _statChangedLogger.StatsChanged += ResetJustAddEffectFlag;
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_effectRemoved);
            BattleEventBus.UnsubscribeEvent(_effectAdded);
            _statChangedLogger.StatsChangedWithSameValue -= LogApplyFail;
            _statChangedLogger.StatsChanged -= ResetJustAddEffectFlag;
        }

        private void LogOnBuffAdded(EffectAddedEvent ctx)
        {
            if (!IsBuffOrDebuff(ctx, out var tag, out var isBuff)) return;
            _isJustAddEffect = true;
            _lastCharacterGotEffect = ctx.Character;
        }

        private void LogApplyFail(Components.Character character, LocalizedString attributeName)
        {
            /// Since effect applied after tag is granted so if there's changed event
            /// it'll be called after LogOnBuffAdded and LogApplyFail will only be raised
            /// when there is a buff and the stats value isnt changed
            if (!_isJustAddEffect || character != _lastCharacterGotEffect) return;
            _isJustAddEffect = false;

            _applyFailMessage.Add(Constants.ATTRIBUTE_NAME, attributeName);
            Logger.QueueLog(_applyFailMessage);
        }

        private void ResetJustAddEffectFlag(Components.Character character, LocalizedString attributeName)
        {
            /// When stat changed successful, reset the just added flag
            if (character != _lastCharacterGotEffect) return; 
            _isJustAddEffect = false;
        }

        private void LogOnBuffRemoved(EffectRemovedEvent ctx)
        {
            if (!IsBuffOrDebuff(ctx, out var tag, out var isBuff)) return;

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

            Logger.QueueLog(message);
        }

        private static bool IsBuffOrDebuff(EffectEvent ctx, out TagScriptableObject tag, out bool isBuff)
        {
            tag = ctx.Tag;
            isBuff = tag.IsChildOf(TagsDef.Buff);
            var isDebuff = tag.IsChildOf(TagsDef.DeBuff);
            return isBuff || isDebuff;
        }
    }
}