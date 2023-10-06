using CryptoQuest.Input;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs.OneButtonDialog
{
    public class UIOneButtonDialog : ModalWindow<UIOneButtonDialog>
    {
        [Header("Child Components")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private Button _defaultSelectButton;
        [SerializeField] private LocalizeStringEvent _messageUi;
        [SerializeField] private LocalizeStringEvent _buttonTextUi;

        private Action _buttonPressed;
        private LocalizedString _message;
        private LocalizedString _buttonText;

        protected override void CheckIgnorableForClose() { }

        private void OnEnable()
        {
            StartCoroutine(CoSelectDefaultButton());
            _inputMediator.MenuCancelEvent += OnButtonPressed;
        }

        private void OnDisable()
        {
            _inputMediator.MenuCancelEvent -= OnButtonPressed;
        }

        private IEnumerator CoSelectDefaultButton()
        {
            yield return null;
            _defaultSelectButton.Select();
        }

        public void OnButtonPressed()
        {
            Debug.Log($"UIOneButtonDialog::Pressed");
            _buttonPressed?.Invoke();
        }

        public UIOneButtonDialog SetButtonsEvent(Action action)
        {
            _buttonPressed = action;
            return this;
        }

        public UIOneButtonDialog SetButtonText(LocalizedString text)
        {
            if(text != null)
            {
                _buttonText = text;
            }    
            return this;
        }    

        public UIOneButtonDialog SetMessage(LocalizedString message)
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
            if(_buttonText != null)
            {
                _buttonTextUi.StringReference = _buttonText;
            }    
        }

        public override UIOneButtonDialog Close()
        {
            Visible = false;
            Destroy(gameObject);
            return this;
        }
    }
}
