using System;
using System.Collections;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs.OneButtonDialog
{
    public class UIOneButtonDialog : AbstractDialog
    {
        [Header("Child Components")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private Button _defaultSelectButton;
        [SerializeField] private LocalizeStringEvent _messageUi;
        [SerializeField] private LocalizeStringEvent _buttonTextUi;

        private Action _buttonPressed;
        private LocalizedString _message;
        private LocalizedString _buttonText;

        private void OnEnable()
        {
            _inputMediator.MenuCancelEvent += OnButtonPressed;
        }

        private void OnDisable()
        {
            _inputMediator.MenuCancelEvent -= OnButtonPressed;
        }

        public override void Show()
        {
            base.Show();
            UpdateUIMessage();
            StartCoroutine(CoSelectDefaultButton());
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
            Hide();
        }

        public UIOneButtonDialog WithButtonCallback(Action action)
        {
            _buttonPressed = action;
            return this;
        }

        public UIOneButtonDialog WithButtonText(LocalizedString text)
        {
            if(text != null)
            {
                _buttonText = text;
            }    
            return this;
        }    

        public UIOneButtonDialog WithMessage(LocalizedString message)
        {
            _message = message;
            return this;
        }

        private void UpdateUIMessage()
        {
            _messageUi.StringReference = _message;
            if(_buttonText != null)
            {
                _buttonTextUi.StringReference = _buttonText;
            }    
        }
    }
}
