using System.Collections;
using CryptoQuest.Battle.Presenter.Commands;
using CryptoQuest.UI.Dialogs.DialogWithCharacterName;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.Components.EnemyComponents.CustomBehaviours
{
    public class PresentDialogCommand : IPresentCommand
    {
        // Match with battle auto hide
        private const float AUTO_HIDE_DURATION = 2f;

        private LocalizedString _localizedMessage;
        private UICharacterNamedDialog _dialog;
        private bool _isHidden;

        public PresentDialogCommand(UICharacterNamedDialog dialog, LocalizedString message)
        {
            _localizedMessage = message;
            _dialog = dialog;
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
                .WithAutoHide(AUTO_HIDE_DURATION)
                .WithHideCallback(HideDialog)
                .Show();
            yield return new WaitUntil(() => _isHidden);
        }

        private void HideDialog()
        {
            _isHidden = true;
        }
    }
}