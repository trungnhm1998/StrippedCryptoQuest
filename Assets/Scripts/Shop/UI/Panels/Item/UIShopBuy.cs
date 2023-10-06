using System.Collections;
using CryptoQuest.Shop.UI.Item;
using CryptoQuest.Shop.UI.Panels.PreviewCharacter;
using UnityEngine;

namespace CryptoQuest.Shop.UI.Panels.Item
{
    public class UIShopBuy : UIShop
    {
        [SerializeField] private UIPreviewCharacterPanel _uiPreviewCharacterPanel;
        protected ShopItemTable _shopItemTable;

        public override void Show()
        {
            _content.SetActive(true);
        }

        protected override void OnItemSelected(IShopItem shopItemInfo)
        {
            base.OnItemSelected(shopItemInfo);
            shopItemInfo.PreviewStat(_uiPreviewCharacterPanel);
        }

        public void SetItemShopTable(ShopItemTable shopItemTable)
        {
            _shopItemTable = shopItemTable;
            ResetShopItem();
            InitItem();
        }

        private IEnumerator LoadItemTable(ShopItemTable shopItemTable)
        {
            yield return shopItemTable.LoadItem(CreateItem);
            yield return SelectDefaultButton();
        }

        private void CreateItem(IShopItem shopItemData)
        {
            InstantiateItem(shopItemData, true);
        }    

        protected override void InitItem()
        {
            StartCoroutine(LoadItemTable(_shopItemTable));
        }
    }
}
