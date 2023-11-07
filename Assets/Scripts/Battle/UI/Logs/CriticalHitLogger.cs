using System.Collections.Generic;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class CriticalHitLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _criticalLog;
        private readonly Dictionary<AttributeScriptableObject, LocalizedString> _attributesToNameDict = new();
        private TinyMessageSubscriptionToken _criticalEvent;

        private void Awake()
        {
            _criticalEvent = BattleEventBus.SubscribeEvent<CriticalHitEvent>(LogCritical);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_criticalEvent);
        }

        private void LogCritical(CriticalHitEvent ctx)
        {
            var msg = new LocalizedString(_criticalLog.TableReference, _criticalLog.TableEntryReference);
            msg.Add(Constants.CHARACTER_NAME, new StringVariable() { Value = ctx.Character.DisplayName });
            Logger.QueueLog(msg);
        }
    }
}