using System;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class ConfirmUpgradePresenter : MonoBehaviour
    {
        public event Action ComfirmedUpgrade;
        public event Action CancelUpgrade;

        [SerializeField] private ConfigUpgradePresenter _configPresenter;
        [SerializeField] private UIConfirmDetails _confirmUI;
        [SerializeField] private BlackSmithDialogsPresenter _dialogManager;
        [SerializeField] private GameObject _confirmUIObject;

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
            _confirmUIObject.SetActive(true);
            _confirmUI.SetupUI(_configPresenter.LevelToUpgrade, _configPresenter.GoldNeeded);
            RegistEvents();
        }

        public void Hide()
        {
            _dialogManager.HideConfirmDialog();
            _confirmUIObject.SetActive(false);
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