using System;
using System.Collections;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs.YesNoDialog
{
    public class UIYesNoDialog : ModalWindow<UIYesNoDialog>
    {
        [Header("Child Components")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private Button _defaultSelectButton;
        [SerializeField] private LocalizeStringEvent _messageUi;

        private Action _yesPressed;
        private Action _noPressed;
        private LocalizedString _message;

        protected override void CheckIgnorableForClose() { }

        private void OnEnable()
        {
            StartCoroutine(CoSelectDefaultButton());
            _inputMediator.MenuCancelEvent += OnNoButtonPressed;
        }

        private void OnDisable()
        {
            _inputMediator.MenuCancelEvent -= OnNoButtonPressed;
        }

        private IEnumerator CoSelectDefaultButton()
        {
            yield return null;
            _defaultSelectButton.Select();
        }

        public void OnYesButtonPressed()
        {
            Debug.Log($"UIYesNoDialog::YesPressed");
            _yesPressed?.Invoke();
        }

        public void OnNoButtonPressed()
        {
            Debug.Log($"UIYesNoDialog::RequestCloseDialog");
            _noPressed?.Invoke();
        }

        public UIYesNoDialog SetButtonsEvent(Action yes, Action no)
        {
            _yesPressed = yes;
            _noPressed = no;
            return this;
        }

        public UIYesNoDialog SetMessage(LocalizedString message)
        {
            _message = message;
            return this;
        }

        protected override void OnBeforeShow()
        {
            UpdateUIMessage();
        }

        private void UpdateUIMessage()
        {
            _messageUi.StringReference = _message;
        }

        public override UIYesNoDialog Close()
        {
            if (this == null) return this;
            Visible = false;
            Destroy(gameObject);
            return this;
        }
    }
}