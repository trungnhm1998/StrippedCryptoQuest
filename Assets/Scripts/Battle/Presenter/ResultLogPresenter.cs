using System.Collections;
using CryptoQuest.Battle.Presenter.Commands;
using CryptoQuest.UI.Dialogs.BattleDialog;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.Presenter
{
    public class ResultLogPresenter : IPresentCommand
    {
        private PresentBattleResultLog _logPresenter;
        private LocalizedString _message;
        private UIGenericDialog _dialog;

        public ResultLogPresenter(PresentBattleResultLog presenter, LocalizedString message, UIGenericDialog dialog)
        {
            _logPresenter = presenter;
            _message = message;
            _dialog = dialog;
        }

        public IEnumerator Present()
        {
            yield return _logPresenter.CoShowLog(_message);
            yield return new WaitUntil(() => _dialog.Content.activeSelf == false);
        }
    }
}