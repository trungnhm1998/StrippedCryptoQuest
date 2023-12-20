using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.Presenter;
using TinyMessenger;
using UnityEngine.Localization;

namespace CryptoQuest.Battle
{
    public class PresentWonLog : PresentBattleResultLog
    {
        private TinyMessageSubscriptionToken _turnWornEvent;

        private void OnEnable()
        {
            _turnWornEvent = BattleEventBus.SubscribeEvent<TurnWonEvent>(OnHandleWon);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_turnWornEvent);
        }

        private void OnHandleWon(TurnWonEvent _)
        {
            if (_battleBus.CurrentBattlefield == null) return;

            LocalizedString winPrompt = _battleBus.CurrentBattlefield.BattlefieldPrompts.WinPrompt;
            if (winPrompt.IsEmpty) return;
            _roundEventsPresenter.EnqueueCommand(new ResultLogPresenter(this, winPrompt, _dialog));
        }
    }
}