using System;
using System.Collections;
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
        [SerializeField] private Button _defaultSelectButton;

        private Action _yesPressed;
        private Action _noPressed;

        private void Start()
        {
            StartCoroutine(CoSelectDefaultButton());
        }

        private void OnDestroy()
        {
            StopCoroutine(CoSelectDefaultButton());
        }

        private IEnumerator CoSelectDefaultButton()
        {
            yield return new WaitUntil(() => Content.activeSelf);
            _defaultSelectButton.Select();
        }

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