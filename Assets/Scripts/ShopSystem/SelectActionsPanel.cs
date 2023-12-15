using CryptoQuest.Gameplay;
using CryptoQuest.Merchant;
using CryptoQuest.UI.Dialogs.BattleDialog;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.ShopSystem
{
    public class SelectActionsPanel : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _closeSystemEvent;
        [SerializeField] private ShopSystemBase _shopSystem;
        [SerializeField] private GameObject _sellPanel;
        [SerializeField] private GameObject _buyPanel;

        [Space]
        [SerializeField] private MerchantInput _input;
        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private LocalizedString _welcomeString = new("ShopUI", "DIALOG_WELCOME");
        [SerializeField] private LocalizedString _goodByeString = new("ShopUI", "DIALOG_EXIT");

        private UIGenericDialog _dialog;

        private void OnEnable()
        {
            _sellPanel.gameObject.SetActive(false);
            _buyPanel.gameObject.SetActive(false);

            _input.CancelEvent += OnCloseSystem;

            if (_dialog != null)
            {
                ShowWelcome(_dialog);
                return;
            }

            GenericDialogController.Instance.Instantiate(ShowWelcome, false);
        }

        private void ShowWelcome(UIGenericDialog dialog)
        {
            _gameState.UpdateGameState(EGameState.Merchant);
            _dialog = dialog;
            dialog
                .WithMessage(_welcomeString)
                .Show();
        }

        private void OnDisable()
        {
            _input.CancelEvent -= OnCloseSystem;
            _dialog
                .WithMessage(_goodByeString)
                .RequireInput()
                .WithHideCallback(() =>
                {
                    GenericDialogController.Instance.Release(_dialog);
                    _gameState.UpdateGameState(EGameState.Field);
                })
                .Show();
        }

        private void OnCloseSystem()
        {
            _closeSystemEvent.RaiseEvent();
        }
    }
}