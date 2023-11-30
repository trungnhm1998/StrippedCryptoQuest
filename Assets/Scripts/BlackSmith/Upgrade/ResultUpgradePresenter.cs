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
        [SerializeField] private GameObject _resultUI;

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
            _resultUI.SetActive(true);
            RegistEvents();
        }

        public void Hide()
        {
            _resultUI.SetActive(false);
            UnRegistEvent();
        }

        private void OnConfirmResult()
        {
            OnConfirmedResult?.Invoke();
        }
    }
}