using System.Collections;
using CryptoQuest.Battle.Presenter.Commands;
using CryptoQuest.Input;
using CryptoQuest.UI.Dialogs.DialogWithCharacterName;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.Components.EnemyComponents.CustomBehaviours
{
    public class PresentDialogCommand : IPresentCommand
    {
        private BattleInput _battleInput;
        private LocalizedString _localizedMessage;
        private UICharacterNamedDialog _dialog;
        private float _autoHideDuration;
        private bool _isHidden;

        public PresentDialogCommand(UICharacterNamedDialog dialog, LocalizedString message,
            BattleInput battleInput, float autoHideDuration=3f)
        {
            _localizedMessage = message;
            _dialog = dialog;
            _autoHideDuration = autoHideDuration;
            _battleInput = battleInput;
        }

        public PresentDialogCommand(LocalizedString message)
        {
            _localizedMessage = message;
        }

        public IEnumerator Present()
        {
            var handle = _localizedMessage.GetLocalizedStringAsync();
            yield return handle;
            var splitedStrings = handle.Result.Split(":");
            if (splitedStrings.Length < 2) yield break;
            
            _dialog
                .WithHeader(splitedStrings[0])
                .WithMessage(splitedStrings[1])
                .WithAutoHide(_autoHideDuration)
                .RequireInput()
                .WithHideCallback(HideDialog)
                .Show();
            yield return new WaitUntil(() => _isHidden);
        }

        private void HideDialog()
        {
            _isHidden = true;
            _battleInput.EnableBattleInput();
        }
    }
}