using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Item;
using CryptoQuest.Shop.UI.Item;
using CryptoQuest.Shop.UI.Panels.Item;
using CryptoQuest.Shop.UI.Panels.Result;
using CryptoQuest.Shop.UI.ScriptableObjects;
using CryptoQuest.Shop.UI.ShopStates;
using CryptoQuest.Shop.UI.ShopStates.Sell;
using FSM;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Shop.UI.Panels.Sell
{
    public class UISellPanel : UIShopPanel
    {
        [SerializeField] private LocalizedString _sellMessage;
        [SerializeField] private YesNoDialogEventChannelSO _yesNoDialogEventChannelSO;
        [SerializeField] private GameObject _helpNavigateInfo;

        [SerializeField] private UIShopInventoryTabHeader _inventoryTabHeader;
        [SerializeField] private UIShop[] _itemLists;

        [SerializeField] private UIResultPanel _resultPanel;

        private readonly Dictionary<ShopStateSO, int> _itemListCache = new();
        private UIShop _currentShop;
        private IShopItem _currentSellItem;
        private ShopManager _shopManager;
        private bool _isSelling = false;


        private int _currentTabIndex;
        private int CurrentTabIndex
        {
            get => _currentTabIndex;
            set
            {
                if (value < 0)
                {
                    _currentTabIndex = _itemLists.Length - 1;
                }
                else if (value >= _itemLists.Length)
                {
                    _currentTabIndex = 0;
                }
                else
                {
                    _currentTabIndex = value;
                }
            }
        }

        public override StateBase<string> GetPanelState(ShopManager shopManager)
        {
            _shopManager = shopManager;
            return new ShopSellState(this);
        }

        private void OnEnable()
        {
            _currentTabIndex = 0;
            _inventoryTabHeader.OpeningTab += ShowItemsWithType;
        }

        private void OnDestroy()
        {
            _inventoryTabHeader.OpeningTab -= ShowItemsWithType;
            for (var index = 0; index < _itemLists.Length; index++)
            {
                var itemList = _itemLists[index];
                itemList.OnSubmit -= OnSubmitSell;
            }
        }

        private void Awake()
        {
            for (var index = 0; index < _itemLists.Length; index++)
            {
                var itemList = _itemLists[index];
                itemList.OnSubmit += OnSubmitSell;
                _itemListCache.Add(itemList.ShopState, index);
            }
        }

        protected override void OnShow()
        {
            _helpNavigateInfo.gameObject.SetActive(true);
            ShowItemsWithType(CurrentTabIndex);
        }

        protected override void OnHide()
        {
            _helpNavigateInfo.gameObject.SetActive(false);
            _currentShop.Hide();
            if(_isSelling)
            {
                _yesNoDialogEventChannelSO.Hide();
            }    
        }

        private void ShowItemsWithType(ShopStateSO itemType)
        {
            var index = _itemListCache[itemType];
            ShowItemsWithType(index);
        }

        private void ShowItemsWithType(int tabIndex)
        {
            if (_currentShop) _currentShop.Hide();
            CurrentTabIndex = tabIndex;
            _inventoryTabHeader.HighlightTab(_itemLists[tabIndex].ShopState);
            _currentShop = _itemLists[tabIndex];
            _currentShop.Show();
        }

        public void ChangeTab(float direction)
        {
            CurrentTabIndex += (int)direction;
            ShowItemsWithType(CurrentTabIndex);
        }

        private void OnSubmitSell(IShopItem shopItemData)
        {
            _isSelling = true;
            _currentSellItem = shopItemData;
            _shopManager.HideDialog();

            _sellMessage.Arguments = new object[] { _currentSellItem.SellPrice };
            _yesNoDialogEventChannelSO.SetMessage(_sellMessage);
            _yesNoDialogEventChannelSO.Show(OnPressYesButtonDialog, OnPressNoButtonDialog);
        }

        private void OnPressYesButtonDialog()
        {
            _isSelling = false;
            _yesNoDialogEventChannelSO.Hide();

            var isSuccess = _shopManager.RequestSell(_currentSellItem);
            _resultPanel.InitResult(isSuccess, State.name);
            _shopManager.RequestChangeState(ShopStateMachine.Result);
        }

        private void OnPressNoButtonDialog()
        {
            _isSelling = false;
            _yesNoDialogEventChannelSO.Hide();
            _shopManager.ShowDialog(DiaglogMessage);
            ShowItemsWithType(CurrentTabIndex);
        }

        public bool RequestGoBack()
        {
            if (_isSelling)
            {
                OnPressNoButtonDialog();
                return false;
            }

            return true;
        }
    }
}
