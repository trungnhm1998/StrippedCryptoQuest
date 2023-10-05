using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.MetadTransferSection
{
    public class UIMetadSection : UITransferSection
    {
        [SerializeField] private LocalizedString _transferMessageSuccess;
        [SerializeField] private LocalizedString _transferMessageFail;
        [SerializeField] private Button _defaultSelection;
        [SerializeField] private List<UIWalletButtons> _walletButtons;
        [SerializeField] private TMP_InputField _inputField;
        private bool _isSelectedButton;
        private UIWalletButtons _currentWallet;
        private IMetadModel _model;

        private void Awake()
        {
            _model = GetComponent<IMetadModel>();
        }

        public override void EnterTransferSection()
        {
            SubscribeButtonEvents();
            base.EnterTransferSection();
        }

        public override void ExitTransferSection()
        {
            ResetTransfer();
            UnsubscribeEvents();
            base.ExitTransferSection();
        }

        private void SubscribeButtonEvents()
        {
            _model.OnSendSuccess += NotifySuccess;
            _model.OnSendFailed += NotifyFailed;

            foreach (var button in _walletButtons)
            {
                button.SelectedEvent += OpenTab;
            }
        }

        private void UnsubscribeEvents()
        {
            _model.OnSendSuccess -= NotifySuccess;
            _model.OnSendFailed -= NotifyFailed;

            foreach (var button in _walletButtons)
            {
                button.SelectedEvent -= OpenTab;
            }
        }

        public void InitMetadTransferSection()
        {
            SetActiveAllButton(true);
            UnHighlightAllButtons();
            _inputField.text = "0";
            _isSelectedButton = false;
            _defaultSelection.Select();
        }

        public override void SendItems()
        {
            if (!_isSelectedButton) return;

            base.SendItems();
            _yesNoDialogEventSO.SetMessage(_currentWallet.SendingMessage);
            SetActiveAllButton(false);
        }

        protected override void YesButtonPressed()
        {
            base.YesButtonPressed();
            float quantityInput = float.Parse(_inputField.text);
            _currentWallet.Send(quantityInput);
            ResetTransfer();
        }

        protected override void NoButtonPressed()
        {
            base.NoButtonPressed();
            ResetTransfer();
        }

        public override void ResetTransfer()
        {
            InitMetadTransferSection();
        }

        private void OpenTab(UIWalletButtons button)
        {
            _currentWallet = button;
            _isSelectedButton = true;
            _inputField.Select();
            _currentWallet.SetHighlight(true);
        }

        private void SetActiveAllButton(bool isActive)
        {
            foreach (var wallet in _walletButtons)
            {
                wallet.Button.interactable = isActive;
            }
        }

        private void UnHighlightAllButtons()
        {
            foreach (var button in _walletButtons)
            {
                button.SetHighlight(false);
            }
        }

        private void NotifySuccess()
        {
            //TODO Nofity when success
        }

        private void NotifyFailed()
        {
            //TODO Notifi when fail
        }
    }
}