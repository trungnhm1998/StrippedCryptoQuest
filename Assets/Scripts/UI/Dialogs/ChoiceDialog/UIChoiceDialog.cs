using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace CryptoQuest.UI.Dialogs.ChoiceDialog
{
    public class UIChoiceDialog : AbstractDialog
    {
        [Header("UI")]
        [SerializeField] private LocalizeStringEvent _messageUi;

        private Action _yesPressed;
        private Action _noPressed;

        public void OnYesButtonPressed()
        {
            Debug.Log($"UIChoiceDialog::YesPressed");
            _yesPressed?.Invoke();
        }

        public void OnNoButtonPressed()
        {
            Debug.Log($"UIChoiceDialog::RequestCloseDialog");
            _noPressed?.Invoke();
        }

        public UIChoiceDialog SetButtonsEvent(Action yes, Action no)
        {
            _yesPressed = yes;
            _noPressed = no;
            return this;
        }

        private LocalizedString _message;

        public UIChoiceDialog SetMessage(LocalizedString message)
        {
            _message = message;
            UpdateUIMessage();
            return this;
        }

        private void UpdateUIMessage()
        {
            _messageUi.StringReference = _message;
        }
    }
}