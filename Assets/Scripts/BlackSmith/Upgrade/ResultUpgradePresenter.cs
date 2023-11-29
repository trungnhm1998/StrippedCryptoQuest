using System;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class ResultUpgradePresenter : MonoBehaviour
    {
        public event Action OnConfirmedResult;

        [SerializeField] private BlackSmithDialogsPresenter _dialogManager;
        [SerializeField] private MerchantsInputManager _input;

        [SerializeField] private LocalizedString _resultMessage;

        private void OnDisable()
        {
            UnRegistEvent();
        }

        private void RegistEvents()
        {
            _input.SubmitEvent += OnConfirmResult;
        }

        private void UnRegistEvent()
        {
            _input.SubmitEvent -= OnConfirmResult;
        }

        public void Show()
        {
            _dialogManager.Dialogue.Hide();
            _dialogManager.Dialogue.SetMessage(_resultMessage).Show();
            RegistEvents();
        }

        public void Hide()
        {
            UnRegistEvent();
        }

        private void OnConfirmResult()
        {
            OnConfirmedResult?.Invoke();
        }
    }
}