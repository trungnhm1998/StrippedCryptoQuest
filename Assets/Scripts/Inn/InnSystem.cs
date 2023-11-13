using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using CryptoQuest.UI.Dialogs.Dialogue;
using Inn.ScriptableObject;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace CryptoQuest.Inn
{
    public class InnSystem : MonoBehaviour
    {
        [Header("Inn Settings")]
        [SerializeField] private float _innCost = 10f;

        [Header("Listening on Channels")]
        [SerializeField] private ShowInnEventChannelSO _showInn;

        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private CurrencySO _currency;

        private UIChoiceDialog _choiceDialog;
        private UIDialogueForGenericMerchant _genericDialog;

        private float _currentGold;

        private void OnEnable() => _showInn.EventRaised += ShowInnRequested;
        private void OnDisable() => _showInn.EventRaised -= ShowInnRequested;
        private void ShowInnRequested() => ChoiceDialogController.Instance.Instantiate(ShowConfirmDialog);

        private void ShowConfirmDialog(UIChoiceDialog dialog)
        {
            _choiceDialog = dialog;

            _inputMediator.EnableDialogueInput();
            LocalizedString message = GetMessage(InnLocalizationTable.INN_CONFIRM);

            message.Arguments = new object[] { _innCost };

            _choiceDialog
                .SetMessage(message)
                .WithYesCallback(Confirm)
                .WithNoCallback(Cancel)
                .Show();
        }

        private void Confirm()
        {
            _currentGold = _wallet[_currency].Amount;

            GenericMerchantDialogueController.Instance.Instantiate(_currentGold < _innCost
                ? ShowFailMessage
                : ShowConfirmMessage);
        }

        private void Cancel()
        {
            ChoiceDialogController.Instance.Release(_choiceDialog);
            _inputMediator.EnableMapGameplayInput();
        }

        private void ShowFailMessage(UIDialogueForGenericMerchant dialog)
        {
            _genericDialog = dialog;

            LocalizedString message = GetMessage(InnLocalizationTable.INN_DO_NOT_ENOUGH_GOLD);

            _genericDialog
                .SetMessage(message)
                .Show();

            _inputMediator.EnableMenuInput();
            _inputMediator.MenuConfirmedEvent += HideDialog;
        }

        private void ShowConfirmMessage(UIDialogueForGenericMerchant dialog)
        {
            _genericDialog = dialog;

            LocalizedString message = GetMessage(InnLocalizationTable.INN_AGREE);

            _genericDialog
                .SetMessage(message)
                .Show();

            _wallet[_currency].UpdateCurrencyAmount(_currentGold - _innCost);

            _inputMediator.EnableMenuInput();
            _inputMediator.MenuConfirmedEvent += HideDialog;
        }

        private void HideDialog()
        {
            _inputMediator.MenuConfirmedEvent -= HideDialog;

            GenericMerchantDialogueController.Instance.Release(_genericDialog);
            ChoiceDialogController.Instance.Release(_choiceDialog);

            _inputMediator.EnableMapGameplayInput();
        }

        private LocalizedString GetMessage(TableEntryReference tableRef)
        {
            LocalizedString welcomeMessage = new LocalizedString(
                InnLocalizationTable.INN_TABLE_REFERENCE,
                tableRef);

            return welcomeMessage;
        }
    }
}