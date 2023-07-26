using System;
using System.Collections;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs.YesNoDialog
{
    public class UIYesNoDialog : ModalWindow<UIYesNoDialog>
    {
        [Header("Child Components")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private Button _defaultSelectButton;

        public Action YesPressed;
        public Action NoPressed;

        protected override void OnBeforeShow() { }

        protected override void CheckIgnorableForClose() { }

        private void OnEnable()
        {
            StartCoroutine(CoSelectDefaultButton());
            _inputMediator.CancelEvent += OnNoButtonPressed;
        }

        private void OnDisable()
        {
            _inputMediator.CancelEvent -= OnNoButtonPressed;
        }

        private IEnumerator CoSelectDefaultButton()
        {
            yield return null;
            _defaultSelectButton.Select();
        }

        public void OnYesButtonPressed()
        {
            YesPressed.Invoke();
        }

        public void OnNoButtonPressed()
        {
            NoPressed.Invoke();
        }
    }
}
