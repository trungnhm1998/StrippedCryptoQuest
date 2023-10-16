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

        private UIDialogueForGenericMerchant _dialog;
        public UIDialogueForGenericMerchant Dialog { get => _dialog; }

        public void BlackSmithOpened()
        {
            GenericMerchantDialogueController.Instance.Instantiate(DialogInstantiated, false);
        }

        private void DialogInstantiated(UIDialogueForGenericMerchant dialog)
        {
            _dialog = dialog;
            _dialog
                .SetMessage(_message)
                .Show();
        }
    }
}