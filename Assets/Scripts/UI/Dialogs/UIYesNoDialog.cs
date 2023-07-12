using System;
using System.Collections;
using CryptoQuest.Events.UI;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs
{
    public class UIYesNoDialog : ModalWindow<UIYesNoDialog>
    {
        [Header("Child Components")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private Button _defaultSelectButton;
        [SerializeField] private DialogCallbackEventSO _yesButtonPressedEvennt;
        [SerializeField] private DialogCallbackEventSO _noButtonPressedEvent;

        public Action YesPressed;
        public Action NoPressed;

        protected override void OnBeforeShow()
        {
            return;
        }

        protected override void CheckIgnorableForClose()
        {
            return;
        }

        public override UIYesNoDialog Close()
        {
            gameObject.SetActive(false);
            return base.Close();
        }

        private void OnEnable()
        {
            StartCoroutine(CoSelectYesButton());
            _inputMediator.CancelEvent += OnNoButtonPressed;
        }

        private void OnDisable()
        {
            _inputMediator.CancelEvent -= OnNoButtonPressed;
        }

        private IEnumerator CoSelectYesButton()
        {
            yield return new WaitForSeconds(.03f);
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
