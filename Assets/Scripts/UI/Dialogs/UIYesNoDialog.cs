using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CryptoQuest.UI.Dialogs
{
    public class UIYesNoDialog : ModalWindow<UIYesNoDialog>
    {
        [Header("Child Components")]
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private Button _defaultSelectButton;

        public event UnityAction YesButtonPressed;
        public event UnityAction NoButtonPressed;

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
            YesButtonPressed?.Invoke();
        }

        public void OnNoButtonPressed()
        {
            NoButtonPressed?.Invoke();
        }
    }
}
