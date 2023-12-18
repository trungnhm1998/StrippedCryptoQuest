using System;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs.ChoiceDialog
{
    public class UIChoiceDialog : AbstractDialog
    {
        [SerializeField] private InputMediatorSO _input;

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

        public UIChoiceDialog SetMessage(LocalizedString message)
        {
            _messageUi.StringReference = message;
            return this;
        }

        public override void Show()
        {
            base.Show();
            _input.EnableMenuInput();
            _input.MenuCancelEvent += OnNoButtonPressed;

            Invoke(nameof(SelectDefaultButton), 0);
        }

        private Action _hideCallback;

        public UIChoiceDialog WithHideCallback(Action callback)
        {
            _hideCallback = callback;
            return this;
        }

        public override void Hide()
        {
            _input.MenuCancelEvent -= OnNoButtonPressed;
            _hideCallback?.Invoke();
            _hideCallback = null;

            base.Hide();
        }

        private void SelectDefaultButton() => _defaultSelectedButton.Select();
    }
}