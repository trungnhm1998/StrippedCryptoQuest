using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Input;
using CryptoQuest.System;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using CryptoQuest.UI.Dialogs.Dialogue;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
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

        [Header("Wallet Settings")]
        [SerializeField] private WalletSO _wallet;

        [SerializeField] private CurrencySO _currency;

        [Header("Fade Config")]
        [SerializeField] private TransitionEventChannelSO _requestTransition;

        [SerializeField] private AbstractTransition _fadeInTransition;
        [SerializeField] private AbstractTransition _fadeOutTransition;

        [Header("Ability System")]
        [SerializeField] private GameplayEffectDefinition _effectDefinition;

        [Header("Listening on Channels")]
        [SerializeField] private ShowInnEventChannelSO _showInn;

        [SerializeField] private InputMediatorSO _inputMediator;

        private UIChoiceDialog _choiceDialog;
        private UIDialogueForGenericMerchant _genericDialog;

        private float _currentGold;
        private IPartyController _party;

        private void Start() => _party = ServiceProvider.GetService<IPartyController>();
        private void OnEnable() => _showInn.EventRaised += ShowInnRequested;
        private void OnDisable() => _showInn.EventRaised -= ShowInnRequested;
        private void ShowInnRequested() => ChoiceDialogController.Instance.Instantiate(ShowWelcomeDialog);

        private void ShowWelcomeDialog(UIChoiceDialog dialog)
        {
            _choiceDialog = dialog;

            _inputMediator.EnableDialogueInput();
            LocalizedString message = GetMessage(InnLocalizationTable.INN_WELCOME);

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
                ? ShowFailDialog
                : ShowAgreeDialog);
        }

        private void Cancel()
        {
            ChoiceDialogController.Instance.Release(_choiceDialog);
            _inputMediator.EnableMapGameplayInput();
        }

        private void ShowFailDialog(UIDialogueForGenericMerchant dialog)
        {
            _genericDialog = dialog;

            LocalizedString message = GetMessage(InnLocalizationTable.INN_DO_NOT_ENOUGH_GOLD);

            _genericDialog
                .SetMessage(message)
                .Show();

            _inputMediator.EnableMenuInput();
            _inputMediator.MenuConfirmedEvent += HideDialog;
        }

        private void ShowAgreeDialog(UIDialogueForGenericMerchant dialog)
        {
            _genericDialog = dialog;

            LocalizedString message = GetMessage(InnLocalizationTable.INN_AGREE);

            _genericDialog
                .SetMessage(message)
                .Show();

            _wallet[_currency].UpdateCurrencyAmount(_currentGold - _innCost);

            foreach (PartySlot partySlot in _party.Slots)
            {
                if (!partySlot.IsValid() || !partySlot.HeroBehaviour.IsValidAndAlive()) continue;

                HeroBehaviour hero = partySlot.HeroBehaviour;
                AbilitySystemBehaviour abilitySystem = hero.GetComponent<AbilitySystemBehaviour>();

                GameplayEffectSpec effect = abilitySystem.MakeOutgoingSpec(_effectDefinition);

                abilitySystem.ApplyEffectSpecToSelf(effect);
            }

            _inputMediator.EnableMenuInput();
            _inputMediator.MenuConfirmedEvent += HideConfirmDialog;
        }

        private void HideConfirmDialog()
        {
            _inputMediator.MenuConfirmedEvent -= HideConfirmDialog;

            _requestTransition.RaiseEvent(_fadeInTransition);

            GenericMerchantDialogueController.Instance.Release(_genericDialog);
            ChoiceDialogController.Instance.Release(_choiceDialog);

            Invoke(nameof(FadeOut), 1);
        }

        private void FadeOut()
        {
            _requestTransition.RaiseEvent(_fadeOutTransition);

            GenericMerchantDialogueController.Instance.Instantiate(ShowConfirmDialog);
        }

        private void ShowConfirmDialog(UIDialogueForGenericMerchant dialog)
        {
            _genericDialog = dialog;

            LocalizedString message = GetMessage(InnLocalizationTable.INN_CONFIRM);

            _genericDialog
                .SetMessage(message)
                .Show();

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