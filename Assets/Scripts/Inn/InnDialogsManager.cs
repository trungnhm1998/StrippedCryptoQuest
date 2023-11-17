using System;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using CryptoQuest.UI.Dialogs.Dialogue;
using Input;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Inn
{
    public class InnDialogsManager : MonoBehaviour
    {
        public event Action<bool> RequestFadeEvent;
        public event Action RequestHealEvent;
        public event Action RequestHide;

        [SerializeField] private InnPresenter _innPresenter;
        [SerializeField] private InputMediatorSO _inputMediator;

        [field: Header("Wallet Settings")]
        [SerializeField] private WalletSO _wallet;

        [SerializeField] private CurrencySO _gold;

        [Header("Localization")]
        [SerializeField] private LocalizedString _welcomeMessage;

        [SerializeField] private LocalizedString _doNotEnoughGoldMessage;
        [SerializeField] private LocalizedString _agreeMessage;
        [SerializeField] private LocalizedString _confirmMessage;
        [SerializeField] private LocalizedString _cancelMessage;

        private UIDialogueForGenericMerchant _dialogue;
        private UIChoiceDialog _choiceDialog;

        private void OnDisable()
        {
            GenericMerchantDialogueController.Instance.Release(_dialogue);
            ChoiceDialogController.Instance.Release(_choiceDialog);
        }

        public void ShowDialog()
        {
            ChoiceDialogController.Instance.Instantiate(InitChoiceDialog);
            GenericMerchantDialogueController.Instance.Instantiate(InitNormalDialog);
        }

        private void InitNormalDialog(UIDialogueForGenericMerchant dialog)
        {
            _dialogue = dialog;
        }

        private void InitChoiceDialog(UIChoiceDialog dialog)
        {
            _choiceDialog = dialog;
            _inputMediator.EnableDialogueInput();

            _welcomeMessage.Arguments = new object[] { _innPresenter.InnPrice };

            _choiceDialog
                .SetMessage(_welcomeMessage)
                .WithYesCallback(OnConfirm)
                .WithNoCallback(OnCancel)
                .Show();
        }

        private void OnConfirm()
        {
            if (_wallet[_gold].Amount < _innPresenter.InnPrice)
            {
                ShowFailDialog();
                return;
            }

            ShowAgreeDialog();
        }

        private void OnCancel()
        {
            _dialogue
                .SetMessage(_cancelMessage)
                .Show();

            _inputMediator.EnableMenuInput();
            _inputMediator.MenuConfirmedEvent += HideDialog;
        }

        private void ShowFailDialog()
        {
            _dialogue
                .SetMessage(_doNotEnoughGoldMessage)
                .Show();

            _inputMediator.EnableMenuInput();
            _inputMediator.MenuConfirmedEvent += HideDialog;
        }

        private void ShowAgreeDialog()
        {
            _dialogue
                .SetMessage(_agreeMessage)
                .Show();

            RequestHealEvent?.Invoke();

            _inputMediator.EnableMenuInput();
            _inputMediator.MenuConfirmedEvent += HideConfirmDialog;
        }

        private void ShowConfirmDialog()
        {
            _dialogue
                .SetMessage(_confirmMessage)
                .Show();

            _inputMediator.MenuConfirmedEvent += HideDialog;
        }

        private void HideDialog()
        {
            _inputMediator.MenuConfirmedEvent -= HideDialog;

            RequestFadeEvent?.Invoke(false);
            RequestHide?.Invoke();

            _inputMediator.EnableMapGameplayInput();
        }

        private void HideConfirmDialog()
        {
            _inputMediator.MenuConfirmedEvent -= HideConfirmDialog;

            RequestFadeEvent?.Invoke(true);

            Invoke(nameof(FadeOut), 1);
        }

        private void FadeOut()
        {
            ShowConfirmDialog();
            RequestFadeEvent?.Invoke(false);
        }
    }
}