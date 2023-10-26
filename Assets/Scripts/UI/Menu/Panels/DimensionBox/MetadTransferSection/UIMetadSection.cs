using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Input;
using CryptoQuest.UI.Dialogs.OneButtonDialog;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using CryptoQuest.UI.Title;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.MetadTransferSection
{
    public class UIMetadSection : UITransferSection
    {
        [SerializeField] private InputMediatorSO _input;
        [SerializeField] private LocalizedString _transferMessageSuccess;
        [SerializeField] private LocalizedString _transferMessageFail;
        [SerializeField] private Button _defaultSelection;
        [SerializeField] private List<UIWalletButtons> _walletButtons;
        [SerializeField] private InputField _inputField;
        private bool _isSelectedButton;
        private UIWalletButtons _currentWallet;
        private IMetadModel _model;
        private UIOneButtonDialog _resultDialog;
        private float _ingameMetad;
        private float _webMetad;
        private bool _isIngameWallet;
        private bool _isTransferMetad;

        private void Awake()
        {
            _model = GetComponent<IMetadModel>();
        }

        private void OnEnable()
        {
            GenericOneButtonDialogController.Instance.Instantiate(OnDialogCreate);
        }

        private void OnDisable()
        {
            GenericOneButtonDialogController.Instance.Release(_resultDialog);
        }

        private void OnDialogCreate(UIOneButtonDialog dialog)
        {
            _resultDialog = dialog;
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
            _isTransferMetad = false;
            SetActiveAllButton(true);
            UnHighlightAllButtons();
            _inputField.text = "0";
            _isSelectedButton = false;
            _defaultSelection.Select();
            _ingameMetad = _model.GetIngameMetad();
            _webMetad = _model.GetWebMetad();
        }

        public override void SendItems()
        {
            if (!_isSelectedButton || _isTransferMetad) return;
            
            _isTransferMetad = true;
            _yesNoDialogEventSO.SetMessage(_currentWallet.SendingMessage);
            SetActiveAllButton(false);
            base.SendItems();
        }

        protected override void YesButtonPressed()
        {
            base.YesButtonPressed();
            float quantityInput = string.IsNullOrEmpty(_inputField.text) ? 0 : float.Parse(_inputField.text);
            _currentWallet.Send(quantityInput);
            _input.DisableAllInput();
            LoadingController.OnEnableLoadingPanel?.Invoke();
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
            _isIngameWallet = _currentWallet.GetComponent<UIGameWalletButtons>() ? true : false;
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
            _resultDialog.WithMessage(_transferMessageSuccess)
                .WithButtonsEvent(OnDialogPress)
                .Show();
            LoadingController.OnDisableLoadingPanel?.Invoke();
        }

        private void NotifyFailed()
        {
            _resultDialog.WithMessage(_transferMessageFail)
                .WithButtonsEvent(OnDialogPress)
                .Show();
            LoadingController.OnDisableLoadingPanel?.Invoke();
        }

        private void OnDialogPress()
        {
            _resultDialog.Hide();
            ResetTransfer();
            _input.EnableMenuInput();
        }

        public void ValidateInputField()
        {
            if (string.IsNullOrEmpty(_inputField.text)) return;
            
            var pattern = Regex.Match(_inputField.text, @"[^0-9]");
            if (_inputField.text.Substring(0, 1) == pattern.ToString())
            {
                _inputField.text = null;
                return;
            }

            float quantityInput = float.Parse(_inputField.text);
            if (_isIngameWallet && quantityInput > _ingameMetad)
                _inputField.text = _ingameMetad.ToString();

            if (!_isIngameWallet && quantityInput > _webMetad)
                _inputField.text = _webMetad.ToString();
        }
    }
}