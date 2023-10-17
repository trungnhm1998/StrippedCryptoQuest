using CryptoQuest.AbilitySystem;
using CryptoQuest.Battle.Events;
using CryptoQuest.System;
using TinyMessenger;
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
            if (!CanLog(ctx, out var asset)) return;
            LogAbnormalStatus(asset.AddedMessage, ctx);
        }

        private void LogOnEffectAffecting(EffectAffectingEvent ctx)
        {
            if (!CanLog(ctx, out var asset)) return;
            LogAbnormalStatus(asset.AffectMessage, ctx);
        }

        private void LogOnEffectRemoved(EffectRemovedEvent ctx)
        {
            if (!CanLog(ctx, out var asset)) return;
            LogAbnormalStatus(asset.RemoveMessage, ctx);
        }

        private bool CanLog(EffectEvent ctx, out TagAsset asset)
        {
            asset = default;
            return !IsDamageOverTimeOrCrowdControlTag(ctx) && TagAssetProvider.TryGetTagAsset(ctx.Tag, out asset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns>true if child of DoT or CC type</returns>
        private static bool IsDamageOverTimeOrCrowdControlTag(EffectEvent ctx)
        {
            var gameplayTag = ctx.Tag;
            var isDamageOverTimeOrCc = gameplayTag.IsChildOf(TagsDef.DamageOverTime) ||
                                       gameplayTag.IsChildOf(TagsDef.CrowdControl);
            return isDamageOverTimeOrCc && ctx.Character.HasTag(gameplayTag);
        }

        private void LogAbnormalStatus(LocalizedString contextMessage, EffectEvent ctx)
        {
            if (contextMessage.IsEmpty) return;
            contextMessage.Add(Constants.CHARACTER_NAME, new StringVariable()
            {
                Value = ctx.Character.DisplayName
            });
            Logger.AppendLog(contextMessage);
        }
    }
}