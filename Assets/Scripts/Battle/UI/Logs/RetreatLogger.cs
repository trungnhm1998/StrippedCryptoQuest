using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.Logs
{
    public class RetreatLogger : MonoBehaviour
    {
        [SerializeField] private UnityEvent<LocalizedString> _presentLoggerEvent;

        [SerializeField] private LocalizedString _retreatSuccessLog;
        [SerializeField] private LocalizedString _retreatFailLog;
        private TinyMessageSubscriptionToken _retreatSucceedEvent;
        private TinyMessageSubscriptionToken _retreatFailedEvent;

        private void OnEnable()
        {
            _retreatSucceedEvent = BattleEventBus.SubscribeEvent<RetreatSucceedEvent>(OnRetreatSucceed);
            _retreatFailedEvent = BattleEventBus.SubscribeEvent<RetreatFailedEvent>(OnRetreatFailed);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_retreatSucceedEvent);
            BattleEventBus.UnsubscribeEvent(_retreatFailedEvent);
        }

        private void OnRetreatSucceed(RetreatSucceedEvent ctx)
        {
            var characterName = ctx.Character.LocalizedName;
            _retreatSuccessLog.Add(Constants.CHARACTER_NAME, characterName);
            _presentLoggerEvent.Invoke(_retreatSuccessLog);
        }

        private void OnRetreatFailed(RetreatFailedEvent ctx)
        {
            var characterName = ctx.Character.LocalizedName;
            _retreatFailLog.Add(Constants.CHARACTER_NAME, characterName);
            _presentLoggerEvent.Invoke(_retreatFailLog);
        }
    }
}