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
        [SerializeField] private UIResultPanel _resultPanel;
        [SerializeField] private UIShopBuy _uiShopBuy;
        
        private ShopManager _shopManager;
        private IShopItemData _currentBuyItem;

        public override StateBase<string> GetPanelState(ShopManager shopManager)
        {
            _shopManager = shopManager;
            return new ShopBuyState(this);
        }

        private void Awake()
        {
            _uiShopBuy.OnSubmit += OnSubmitBuy;
        }

        private void OnDestroy()
        {
            _uiShopBuy.OnSubmit -= OnSubmitBuy;
        }

        protected override void OnShow()
        {
            _itemInfo.gameObject.SetActive(true);
            _partyInfo.gameObject.SetActive(true);
            ShowItem();
        }

        protected override void OnHide()
        {
            _itemInfo.gameObject.SetActive(false);
            _partyInfo.gameObject.SetActive(false);
        }
        private void OnSubmitBuy(IShopItemData shopItemInfo)
        {
            _currentBuyItem = shopItemInfo;
            _shopManager.HideDialog();

            _buyMessage.Arguments = new object[] { _currentBuyItem.Price };
            _yesNoDialogEventChannelSO.SetMessage(_buyMessage);
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
            ShowItem();
        }

        private void ShowItem()
        {
            _uiShopBuy.SetItemShopTable(_shopManager.ShopInfo);
            _uiShopBuy.Show();
        }    
    }
}
