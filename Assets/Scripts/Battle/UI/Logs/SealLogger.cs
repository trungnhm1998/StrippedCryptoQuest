using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.AbilitySystem;
using CryptoQuest.Battle.Events;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    /// <summary>
    /// Log seal magic, physic effect
    /// </summary>
    public class SealLogger : LoggerComponentBase
    {
        [Serializable]
        private struct TagEffectNameMapping
        {
            public TagScriptableObject Tag;
            public LocalizedString EffectName;
            public LocalizedString SealAddedMessage;
            public LocalizedString SealedMessage;
        }
        [SerializeField] private TagEffectNameMapping[] _tagEffectNameMappings;
        [SerializeField] private LocalizedString _sealRemovedMessage;

        private TinyMessageSubscriptionToken _sealedEvent;
        private TinyMessageSubscriptionToken _effectAdded;
        private TinyMessageSubscriptionToken _effectRemoved;

        private Dictionary<TagScriptableObject, TagEffectNameMapping> _tagEffectNameDict = new();

        private void Awake()
        {
            _tagEffectNameDict = _tagEffectNameMappings.ToDictionary(t => t.Tag);
        }

        private void OnEnable()
        {
            _sealedEvent = BattleEventBus.SubscribeEvent<SealedEvent>(LogOnSealed);
            _effectAdded = BattleEventBus.SubscribeEvent<EffectAddedEvent>(LogOnEffectAdded);
            _effectRemoved = BattleEventBus.SubscribeEvent<EffectRemovedEvent>(LogOnEffectRemoved);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_sealedEvent);
            BattleEventBus.UnsubscribeEvent(_effectAdded);
            BattleEventBus.UnsubscribeEvent(_effectRemoved);
        }

        private void LogOnSealed(SealedEvent ctx)
        {
            if (!_tagEffectNameDict.TryGetValue(ctx.Tag, out var tagMapping)) return;
            var log = new LocalizedString(tagMapping.SealAddedMessage.TableReference,
                tagMapping.SealAddedMessage.TableEntryReference);
            log.Add(Constants.CHARACTER_NAME, new StringVariable()
            {
                Value = ctx.Character.DisplayName
            });
            Logger.QueueLog(log);
        }

        private void LogOnEffectAdded(EffectAddedEvent ctx)
        {
            if (!CanLogWhenActive(ctx)) return;
            var log = CreateSealLog(_tagEffectNameDict[ctx.Tag].SealAddedMessage, ctx);
            Logger.QueueLog(log);
        }

        private void LogOnEffectRemoved(EffectRemovedEvent ctx)
        {
            if (!CanLogWhenRemove(ctx)) return;
            var log = CreateSealLog(_sealRemovedMessage, ctx);
            log.Add(Constants.ABILITY_NAME, _tagEffectNameDict[ctx.Tag].EffectName);
            Logger.QueueLog(log);
        }

        private bool CanLogWhenActive(EffectEvent ctx)
        {
            return _tagEffectNameDict.ContainsKey(ctx.Tag) && ctx.Character.HasTag(ctx.Tag)
                && ctx.Character.IsValidAndAlive();
        }

        private bool CanLogWhenRemove(EffectEvent ctx)
        {
            return _tagEffectNameDict.ContainsKey(ctx.Tag) && !ctx.Character.HasTag(ctx.Tag)
                && ctx.Character.IsValidAndAlive();
        }

        private LocalizedString CreateSealLog(LocalizedString contextMessage, EffectEvent ctx)
        {
            var msg = new LocalizedString(contextMessage.TableReference, contextMessage.TableEntryReference);
            msg.Add(Constants.CHARACTER_NAME, new StringVariable()
            {
                Value = ctx.Character.DisplayName
            });
            return msg;
        }
    }
}