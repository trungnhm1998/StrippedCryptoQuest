using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class MpNotEnoughLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _noManaMessage;
        private TinyMessageSubscriptionToken _noManaEvent;

        private void OnEnable()
        {
            _noManaEvent = BattleEventBus.SubscribeEvent<MpNotEnoughEvent>(LogNoMana);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_noManaEvent);
        }

        private void LogNoMana(MpNotEnoughEvent skillEvent)
        {
            var log = new LocalizedString(_noManaMessage.TableReference,
                _noManaMessage.TableEntryReference);
            Logger.QueueLog(log);
        }
    }
}