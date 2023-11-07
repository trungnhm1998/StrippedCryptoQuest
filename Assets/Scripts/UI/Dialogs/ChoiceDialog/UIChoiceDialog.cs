using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs.ChoiceDialog
{
    public class UIChoiceDialog : AbstractDialog
    {
        [Header("UI")]
        [SerializeField] private LocalizeStringEvent _messageUi;

        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;

        private void Start()
        {
            _defaultSelectedButton = _yesButton;
        }

        private Action _yesPressed;
        private Action _noPressed;

        public void OnYesButtonPressed()
        {
            _yesPressed?.Invoke();
            _yesPressed = null;
            Hide();
        }

        public void OnNoButtonPressed()
        {
            _noPressed?.Invoke();
            _noPressed = null;
            Hide();
        }

        public UIChoiceDialog SetButtonsEvent(Action yes, Action no)
        {
            _yesPressed = yes;
            _noPressed = no;
            return this;
        }

        private Button _defaultSelectedButton;

        public UIChoiceDialog SelectYes()
        {
            _defaultSelectedButton = _yesButton;
            return this;
        }

        public UIChoiceDialog SelectNo()
        {
            _defaultSelectedButton = _noButton;
            return this;
        }

        public UIChoiceDialog WithYesCallback(Action callback)
        {
            _yesPressed = callback;
            return this;
        }

        public UIChoiceDialog WithNoCallback(Action callback)
        {
            _noPressed = callback;
            return this;
        }

        private LocalizedString _message;

        public UIChoiceDialog SetMessage(LocalizedString message)
        {
            _message = message;
            return this;
        }

        public override void Show()
        {
            base.Show();
            _messageUi.StringReference = _message;
            _message = null;

            Invoke(nameof(SelectDefaultButton), 0);
        }

        private void SelectDefaultButton() => _defaultSelectedButton.Select();
    }
}