using System.Collections;
using CryptoQuest.Battle.Presenter;
using CryptoQuest.UI.Dialogs.BattleDialog;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle
{
    public class PresentBattleResultLog : MonoBehaviour
    {
        [SerializeField] protected BattleBus _battleBus;
        [SerializeField] protected RoundEventsPresenter _roundEventsPresenter;
        protected UIGenericDialog _dialog;

        public IEnumerator CoShowLog(LocalizedString message)
        {
            _dialog
                .RequireInput()
                .WithMessage(message);
            yield return _dialog.CoShow();
        }
    }
}