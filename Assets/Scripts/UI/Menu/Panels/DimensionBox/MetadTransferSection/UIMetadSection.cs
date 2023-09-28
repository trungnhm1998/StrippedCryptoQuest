using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.MetadTransferSection
{
    public class UIMetadSection : UITransferSection
    {
        [SerializeField] private UnityEvent<bool> _confirmClickedEvent;
        [SerializeField] private UnityEvent<float, bool> _inputValueEvent;
        [SerializeField] private LocalizedString _transferMessageSuccess;
        [SerializeField] private LocalizedString _transferMessageFail;
        [SerializeField] private Button _defaultSelection;
        [SerializeField] private List<UIWalletButtons> _walletButtons;
        [SerializeField] private TMP_InputField _inputField;
        private bool _isSuccessTransfer;
        private bool _isSelectedButton;
        private EWalletType _walletType;

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

        public override void ResetTransfer()
        {
            Init();
            _yesNoDialogEventSO.Hide();
        }

        private void SubscribeButtonEvents()
        {
            foreach (var button in _walletButtons)
            {
                button.SelectedEvent += OpenTab;
            }
        }

        private void UnsubscribeEvents()
        {
            foreach (var button in _walletButtons)
            {
                button.SelectedEvent -= OpenTab;
            }
        }

        public void Init()
        {
            SetActiveAllButton(true);
            UnhighlightAllButtons();
            _inputField.text = "0";
            _isSelectedButton = false;
            _defaultSelection.Select();
        }

        public override void SendItems()
        {
            if (!_isSelectedButton) return;
            SubmitSendMetad(_inputField.text);
            SetActiveAllButton(false);
            _yesNoDialogEventSO.Show(OnConfirmClicked, OnCancelClicked);
        }

        public void ValidateTransferMetad(bool isSuccess)
        {
            _isSuccessTransfer = isSuccess;
            _yesNoDialogEventSO.SetMessage(_message = isSuccess == true ? _transferMessageSuccess : _transferMessageFail);
        }

        private void OnConfirmClicked()
        {
            _confirmClickedEvent?.Invoke(_isSuccessTransfer);
            ResetTransfer();
        }

        private void OnCancelClicked()
        {
            ResetTransfer();
        }
        private void SubmitSendMetad(string input)
        {
            float quantityMetadInput = float.Parse(input);
            _inputValueEvent.Invoke(quantityMetadInput, _walletType == EWalletType.IngameWallet);
        }

        private void OpenTab(UIWalletButtons button)
        {
            _isSelectedButton = true;
            _inputField.Select();
            button.SetHighlight(true);
            _walletType = button.WalletType;
        }


        private void SetActiveAllButton(bool isActive)
        {
            foreach (var wallet in _walletButtons)
            {
                wallet.Button.interactable = isActive;
            }
        }

        private void UnhighlightAllButtons()
        {
            foreach (var button in _walletButtons)
            {
                button.SetHighlight(false);
            }
        }
    }
}