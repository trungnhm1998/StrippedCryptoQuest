using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.Presenter;
using CryptoQuest.UI.Dialogs.BattleDialog;
using TinyMessenger;
using UnityEngine.Localization;

namespace CryptoQuest.Battle
{
    public class PresentLostLog : PresentBattleResultLog
    {
        private TinyMessageSubscriptionToken _turnLostEvent;

        private void OnEnable()
        {
            _turnLostEvent = BattleEventBus.SubscribeEvent<TurnLostEvent>(OnHandleLost);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_turnLostEvent);
        }

        private void OnHandleLost(TurnLostEvent _)
        {
            if (_battleBus.CurrentBattlefield == null) return;

            LocalizedString losePrompt = _battleBus.CurrentBattlefield.BattlefieldPrompts.LosePrompt;
            if (losePrompt.IsEmpty) return;
            GenericDialogController.Instance.InstantiateAsync((dialog) =>
            {
                _dialog = dialog;
                _roundEventsPresenter.EnqueueCommand(new ResultLogPresenter(this, losePrompt, _dialog));
            });
        }
    }
}