using System;
using System.Collections.Generic;
using CryptoQuest.AbilitySystem;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
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

        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;

        [SerializeField] private LocalizedString _buffLog;
        [SerializeField] private LocalizedString _debuffLog;
        [SerializeField] private LocalizedString _applyFailMessage;
        [SerializeField] private LocalizedString _buffWearOffMessage;
        [SerializeField] private LocalizedString _debuffWearOffMessage;
        [SerializeField] private TagAttributeMapping[] _tagAttributeMappings;

        private TinyMessageSubscriptionToken _effectAdded;
        private TinyMessageSubscriptionToken _effectRemoved;
        private readonly Dictionary<TagScriptableObject, AttributeScriptableObject> _tagAttributeMap = new();

        private EffectAddedEvent _lastCtx;

        private void Awake()
        {
            foreach (var tagAttributeMapping in _tagAttributeMappings)
                _tagAttributeMap.Add(tagAttributeMapping.Tag, tagAttributeMapping.Attribute);
        }

        private void OnEnable()
        {
            _effectRemoved = BattleEventBus.SubscribeEvent<EffectRemovedEvent>(LogOnBuffRemoved);
            _effectAdded = BattleEventBus.SubscribeEvent<EffectAddedEvent>(LogOnBuffAdded);
            _attributeChangeEvent.Changed += OnAttributeChanged;
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_effectRemoved);
            BattleEventBus.UnsubscribeEvent(_effectAdded);
            _attributeChangeEvent.Changed -= OnAttributeChanged;
        }

        private void LogOnBuffAdded(EffectAddedEvent ctx)
        {
            if (!IsBuffOrDebuff(ctx, out var tag, out var isBuff)) return;
            if (!ctx.Character.TryGetComponent(out EffectSystem effectSystem)) return;
            _lastCtx = ctx;
        }

        private void OnAttributeChanged(AttributeSystemBehaviour attributeSystem, AttributeValue oldValue,
            AttributeValue newValue)
        {
            if (!attributeSystem.TryGetComponent(out Components.Character character)) return;

            if (_lastCtx == null || character != _lastCtx.Character) return;

            if (!IsBuffOrDebuff(_lastCtx, out var tag, out var isBuff)) return;

            if (!character.TryGetComponent(out EffectSystem effectSystem)) return;

            var lastestEffect = effectSystem.AppliedEffects[^1];
            var largestEffect = effectSystem.GetLargestGameplayEffectMagnitude(lastestEffect.Spec);

            if (largestEffect != lastestEffect) 
            {
                LogApplyFail(_lastCtx.Tag);
                _lastCtx = null;
                return;
            }

            var message = isBuff ? _buffLog : _debuffLog;
            LogMessage(_lastCtx, message, tag);
            _lastCtx = null;
        }

        private void LogApplyFail(TagScriptableObject tag)
        {
            var applyFailMessage = new LocalizedString();
            applyFailMessage.SetReference(_applyFailMessage.TableReference, _applyFailMessage.TableEntryReference);
            applyFailMessage.Add(Constants.ATTRIBUTE_NAME, GetAttributeName(tag));
            Logger.QueueLog(applyFailMessage);
        }

        private void LogOnBuffRemoved(EffectRemovedEvent ctx)
        {
            if (!IsBuffOrDebuff(ctx, out var tag, out var isBuff)) return;

            var message = isBuff ? _buffWearOffMessage : _debuffWearOffMessage;
            LogMessage(ctx, message, tag);
        }

        private void LogMessage(EffectEvent ctx, LocalizedString message, TagScriptableObject tag)
        {
            if (!ctx.Character.IsValidAndAlive()) return;

            var msg = new LocalizedString(message.TableReference, message.TableEntryReference)
            {
                {
                    Constants.CHARACTER_NAME, new StringVariable()
                    {
                        Value = ctx.Character.DisplayName
                    }
                }
            };
            msg.Add(Constants.ATTRIBUTE_NAME, GetAttributeName(tag));

            Logger.QueueLog(msg);
        }

        private const string ATTRIBUTE_NAME_INVALID = "AttributeNameInvalid";

        private IVariable GetAttributeName(TagScriptableObject tag)
        {
            if (!_tagAttributeMap.TryGetValue(tag, out var attribute) || attribute.DisplayName.IsEmpty)
                return new StringVariable()
                {
                    Value = ATTRIBUTE_NAME_INVALID
                };
            return attribute.DisplayName;
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