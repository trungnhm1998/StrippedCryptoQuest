using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Shop;
using CryptoQuest.Shop.UI.Panels.Item;
using CryptoQuest.Shop.UI.Panels.Result;
using CryptoQuest.Shop.UI.ScriptableObjects;
using CryptoQuest.Shop.UI.ShopStates;
using CryptoQuest.Shop.UI.ShopStates.Buy;
using FSM;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Shop.UI.Panels.Buy
{
    public class UIBuyPanel : UIShopPanel
    {
        [SerializeField] private LocalizedString _buyMessage;
        [SerializeField] private YesNoDialogEventChannelSO _yesNoDialogEventChannelSO;
        [SerializeField] private GameObject _itemInfo;
        [SerializeField] private GameObject _partyInfo;

        [SerializeField] private UIShopBuy[] _itemLists;

        [SerializeField] private UIResultPanel _resultPanel;

        private readonly Dictionary<ETypeShop, int> _itemListCache = new();
        private UIShopBuy _currentShopItem;
        private ShopManager _shopManager;
        private IShopItemData _currentBuyItem;

        public override StateBase<string> GetPanelState(ShopManager shopManager)
        {
            _shopManager = shopManager;
            return new ShopBuyState(this);
        }

        private void Awake()
        {
            for (var index = 0; index < _itemLists.Length; index++)
            {
                var itemList = _itemLists[index];
                itemList.OnSubmit += OnSubmitBuy;
                _itemListCache.Add(itemList.ShopType, index);
            }
        }

        private void OnDestroy()
        {
            for (var index = 0; index < _itemLists.Length; index++)
            {
                var itemList = _itemLists[index];
                itemList.OnSubmit -= OnSubmitBuy;
            }
        }

        protected override void OnShow()
        {
            _itemInfo.gameObject.SetActive(true);
            _partyInfo.gameObject.SetActive(true);

            ShowItemsWithType(_shopManager.ShopInfo.Type);
        }

        protected override void OnHide()
        {
            _itemInfo.gameObject.SetActive(false);
            _partyInfo.gameObject.SetActive(false);
        }

        private void ShowItemsWithType(ETypeShop itemType)
        {
            var index = _itemListCache[itemType];
            ShowItemsWithType(index);
        }

        private void ShowItemsWithType(int tabIndex)
        {
            foreach (var item in _itemLists)
            {
                item.Hide();
            }

            _currentShopItem = _itemLists[tabIndex];
            _currentShopItem.SetItemShopTable(_shopManager.ShopInfo.ItemTable);
            _currentShopItem.Show();
        }

        private void OnSubmitBuy(IShopItemData shopItemInfo)
        {
            _currentBuyItem = shopItemInfo;

            _buyMessage.Arguments = new object[] { _currentBuyItem.Price };
            _shopManager.ShowDialog(_buyMessage);
            _yesNoDialogEventChannelSO.Show(OnPressYesButtonDialog, OnPressNoButtonDialog);
        }

        private void OnPressYesButtonDialog()
        {
            _yesNoDialogEventChannelSO.Hide();
            _resultPanel.InitResult(true, State.name);
            _shopManager.RequestChangeState(ShopStateMachine.Result);
        }

        private void OnPressNoButtonDialog()
        {
            _yesNoDialogEventChannelSO.Hide();
            _shopManager.ShowDialog(DiaglogMessage);
            ShowItemsWithType(0);
        }
    }
}
