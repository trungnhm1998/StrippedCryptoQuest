using CryptoQuest.AbilitySystem;
using CryptoQuest.Battle.Events;
using CryptoQuest.System;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    /// <summary>
    /// Log effect such as poison, burn, paralyze, stun, etc.
    /// </summary>
    public class EffectLogger : LoggerComponentBase
    {
        private ITagAssetProvider _tagAssetProvider;

        private ITagAssetProvider TagAssetProvider =>
            _tagAssetProvider ??= ServiceProvider.GetService<ITagAssetProvider>();

        private TinyMessageSubscriptionToken _effectAffectingMessage;
        private TinyMessageSubscriptionToken _effectAdded;
        private TinyMessageSubscriptionToken _effectRemoved;

        private void OnEnable()
        {
            _effectAdded = BattleEventBus.SubscribeEvent<EffectAddedEvent>(LogOnEffectAdded);
            _effectAffectingMessage = BattleEventBus.SubscribeEvent<EffectAffectingEvent>(LogOnEffectAffecting);
            _effectRemoved = BattleEventBus.SubscribeEvent<EffectRemovedEvent>(LogOnEffectRemoved);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_effectAdded);
            BattleEventBus.UnsubscribeEvent(_effectAffectingMessage);
            BattleEventBus.UnsubscribeEvent(_effectRemoved);
        }

        private void LogOnEffectAdded(EffectAddedEvent ctx)
        {
            if (!CanLogWhenActive(ctx, out var asset)) return;
            LogAbnormalStatus(asset.AddedMessage, ctx);
        }

        private void LogOnEffectAffecting(EffectAffectingEvent ctx)
        {
            if (!CanLogWhenActive(ctx, out var asset)) return;
            LogAbnormalStatus(asset.AffectMessage, ctx);
        }

        private void LogOnEffectRemoved(EffectRemovedEvent ctx)
        {
            if (!CanLogWhenRemove(ctx, out var asset)) return;
            LogAbnormalStatus(asset.RemoveMessage, ctx);
        }

        private bool CanLogWhenActive(EffectEvent ctx, out TagAsset asset)
        {
            asset = default;
            // When effect added and effecting need to check has tag in system
            return TryGetValidTagAsset(ctx, out asset) && ctx.Character.HasTag(ctx.Tag);
        }

        private bool CanLogWhenRemove(EffectEvent ctx, out TagAsset asset)
        {
            asset = default;
            // When removed system has to check all tag has been remove
            return TryGetValidTagAsset(ctx, out asset) && !ctx.Character.HasTag(ctx.Tag);
        }

        private bool TryGetValidTagAsset(EffectEvent ctx, out TagAsset asset)
        {
            var isContextValid = IsDamageOverTimeOrCrowdControlTag(ctx);
            asset = default;
            return isContextValid && TagAssetProvider.TryGetTagAsset(ctx.Tag, out asset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns>true if child of DoT or CC type</returns>
        private static bool IsDamageOverTimeOrCrowdControlTag(EffectEvent ctx)
        {
            var gameplayTag = ctx.Tag;
            return gameplayTag.IsChildOf(TagsDef.DamageOverTime) ||
                gameplayTag.IsChildOf(TagsDef.CrowdControl);
        }

        private void LogAbnormalStatus(LocalizedString contextMessage, EffectEvent ctx)
        {
            if (contextMessage.IsEmpty) return;
            if (ctx.Tag != TagsDef.Dead && !ctx.Character.IsValidAndAlive()) return;

            var msg = new LocalizedString(contextMessage.TableReference, contextMessage.TableEntryReference)
            {
                {
                    Constants.CHARACTER_NAME, new StringVariable()
                    {
                        Value = ctx.Character.DisplayName
                    }
                }
            };
            Logger.QueueLog(msg);
        }
    }
}