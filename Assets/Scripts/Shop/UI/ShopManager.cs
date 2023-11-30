using CryptoQuest.Events;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Input;
using CryptoQuest.Menu;
using CryptoQuest.Shop.UI.Item;
using CryptoQuest.Shop.UI.Panels;
using CryptoQuest.Shop.UI.ScriptableObjects;
using CryptoQuest.Shop.UI.ShopStates;
using CryptoQuest.System;
using IndiGames.Core.Common;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace CryptoQuest.Shop.UI
{
    public class ShopManager : MonoBehaviour
    {
        [Header("Gold Info")]
        [SerializeField] private CurrencySO _goldSO;
        private CurrencyInfo _goldInfo;
        [SerializeField]private Text _goldAmount;

        [Header("Input Config")]
        [SerializeField] private MerchantsInputManager _input;
        [SerializeField] private MenuSelectionHandler _menuSelectionHandler;

        [Header("Shop Config")]
        [SerializeField] private GameObject _shopContent;
        [SerializeField] private GameObject _shopPanelContainer;
        [SerializeField] private ShopStateSO _defaulState;

        [Header("Listen Events")]
        [SerializeField] private ShowShopEventChannelSO _showShopEvent;
        [SerializeField] private LocalizedStringEventChannelSO _showShopDialogEventChannel;
        [SerializeField] private VoidEventChannelSO _hideDialogEventChannel;
        [SerializeField] private LocalizedString _exitMessage;

        public ShopItemTable ShopInfo { get; private set; }

        private ShopStateMachine _shopFsm;
        private IShopInventoryController _shopInventoryController;

        private void Awake()
        {
            SetupMenuStateMachines();
        }

        private void OnEnable()
        {
            _showShopEvent.EventRaised += ShowShop;

            _input.ExitEvent += HideShop;
            _input.CancelEvent += Back;
            _input.ChangeTabEvent += ChangeTab;
            _input.SubmitEvent += Submit;
        }

        private void OnDisable()
        {
            _showShopEvent.EventRaised -= ShowShop;

            _input.ExitEvent -= HideShop;
            _input.CancelEvent -= Back;
            _input.ChangeTabEvent -= ChangeTab;
            _input.SubmitEvent -= Submit;
        }

        private void SetupMenuStateMachines()
        {
            _shopFsm = new ShopStateMachine(this);
            var panels = _shopPanelContainer.GetComponentsInChildren<UIShopPanel>();
            foreach (var panel in panels)
            {
                _shopFsm.AddState(panel.State.name, panel.GetPanelState(this));
            }

            _shopFsm.SetStartState(_defaulState.name);
        }

        #region State Machine Delegates

        private void ChangeTab(float direction)
        {
            _shopFsm.ChangeTab(direction);
        }

        private void Submit()
        {
            _shopFsm.Confirm();
        }

        private void Back()
        {
            _shopFsm.HandleBack();
        }

        #endregion

        private void ShowShop(ShopItemTable shopinfo)
        {
            ShopInfo = shopinfo;
            Initialize();
            _shopContent.SetActive(true);
            _shopFsm.Init();
            _input.EnableInput();
        }

        public void HideShop()
        {
            _shopFsm.OnExit();
            _menuSelectionHandler.Unselect();
            _shopContent.SetActive(false);
            _showShopDialogEventChannel?.RaiseEvent(_exitMessage);
            _input.EnableDialogueInput();
        }

        private void Initialize()
        {
            _shopInventoryController = ServiceProvider.GetService<IShopInventoryController>();
            UpdateGoldAmount();
        }

        private void UpdateGoldAmount()
        {
            // REFACTOR: SHOP
            // _goldAmount.text = _walletControllerSO.Wallet.Gold.Amount.ToString();
        }

        public void ShowDialog(LocalizedString message)
        {
            _showShopDialogEventChannel.RaiseEvent(message);
        }

        public void HideDialog()
        {
            _hideDialogEventChannel.RaiseEvent();
        }

        public void RequestChangeState(string state)
        {
            _shopFsm.RequestStateChange(state);
        }

        public bool RequestBuy(IShopItem item)
        {
            if (item.TryToBuy(_shopInventoryController))
            {
                UpdateGoldAmount();
                return true;
            }

            return false;
        }

        public bool RequestSell(IShopItem item)
        {
            if (item.TryToSell(_shopInventoryController))
            {
                UpdateGoldAmount();
                return true;
            }

            return false;
        }
    }
}
