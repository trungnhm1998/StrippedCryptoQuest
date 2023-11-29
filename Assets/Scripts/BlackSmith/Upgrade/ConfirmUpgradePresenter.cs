using System;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class ConfirmUpgradePresenter : MonoBehaviour
    {
        public event Action ComfirmedUpgrade;
        public event Action CancelUpgrade;

        [SerializeField] private BlackSmithDialogsPresenter _dialogManager;
        [SerializeField] private GameObject _confirmUI;

        [SerializeField] private LocalizedString _confirmUpgradeMessage;

        private void OnDisable()
        {
            UnRegistEvent();
        }

        private void RegistEvents()
        {
            _dialogManager.ConfirmYesEvent += OnConfirmUpgrade;
            _dialogManager.ConfirmNoEvent += OnCancelUpgrade;
        }

        private void UnRegistEvent()
        {
            _dialogManager.ConfirmYesEvent -= OnConfirmUpgrade;
            _dialogManager.ConfirmNoEvent -= OnCancelUpgrade;
        }

        public void Show()
        {
            _dialogManager.Dialogue.Hide();
            _dialogManager.ShowConfirmDialog(_confirmUpgradeMessage);
            _confirmUI.SetActive(true);
            RegistEvents();
        }

        public void Hide()
        {
            _dialogManager.HideConfirmDialog();
            _confirmUI.SetActive(false);
            UnRegistEvent();
        }

        private void OnConfirmUpgrade()
        {
            ComfirmedUpgrade?.Invoke();
        }

        private void OnCancelUpgrade()
        {
            CancelUpgrade?.Invoke();
        }
    }
}