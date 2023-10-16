using System;
using System.Collections;
using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Menu;
using CryptoQuest.System.Dialogue.Events;
using CryptoQuest.UI.Dialogs.Dialogue;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith
{
    public class BlackSmithDialogManager : MonoBehaviour
    {
        [SerializeField] private YesNoDialogEventChannelSO _yesNoDialogEventSO;
        [SerializeField] private LocalizedString _message;

        private UIDialogueForGenericMerchant _dialogue;
        public UIDialogueForGenericMerchant Dialogue { get => _dialogue; }

        public void BlackSmithOpened()
        {
            GenericMerchantDialogueController.Instance.Instantiate(DialogInstantiated, false);
        }

        private void DialogInstantiated(UIDialogueForGenericMerchant dialog)
        {
            _dialogue = dialog;
            _dialogue
                .SetMessage(_message)
                .Show();
        }

        public void ShowConfirmDialog(LocalizedString confirmMessage)
        {
            _yesNoDialogEventSO.SetMessage(confirmMessage);
            _yesNoDialogEventSO.Show(YesButtonPressed, NoButtonPressed);
        }

        private void YesButtonPressed()
        {
            _yesNoDialogEventSO.Hide();
        }

        private void NoButtonPressed()
        {
            _yesNoDialogEventSO.Hide();
        }
    }
}