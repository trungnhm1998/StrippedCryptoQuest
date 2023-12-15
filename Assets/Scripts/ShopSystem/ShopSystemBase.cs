using CryptoQuest.Gameplay;
using CryptoQuest.Merchant;
using CryptoQuest.UI.Dialogs.BattleDialog;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.ShopSystem
{
    public abstract class ShopSystemBase : MerchantSystemBase
    {
        [Header("Configs")]
        [SerializeField] private MerchantInput _input;

        [SerializeField] private GameStateSO _gameState;
        [SerializeField] private LocalizedString _welcomeString = new("ShopUI", "DIALOG_WELCOME");
        [SerializeField] private LocalizedString _goodByeString = new("ShopUI", "DIALOG_EXIT");
        [SerializeField] private PriceMappingDatabase _itemsPriceDatabase;

        [Header("Panels")]
        [SerializeField] private GameObject _selectActionPanel;

        [SerializeField] private GameObject _sellPanel;
        [SerializeField] private GameObject _buyPanel;
        private UIGenericDialog _dialog;

        protected override void OnInit()
        {
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
            OpenSelectionPanel();
        }

        protected override void OnExit()
        {
            _input.CancelEvent -= OnCloseSystem;
            _selectActionPanel.gameObject.SetActive(false);
            _buyPanel.gameObject.SetActive(false);
            _sellPanel.gameObject.SetActive(false);
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
            if (_selectActionPanel.activeSelf == false) return;
            CloseSystemEvent.RaiseEvent();
        }

        public void OpenSelectionPanel()
        {
            _selectActionPanel.gameObject.SetActive(true);
            _buyPanel.gameObject.SetActive(false);
            _sellPanel.gameObject.SetActive(false);
        }
    }
}