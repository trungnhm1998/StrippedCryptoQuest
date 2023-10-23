using System.Collections.Generic;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public class UIWalletEquipmentList : UIEquipmentList
    {
        private List<INFT> _walletEquipmentList = new List<INFT>();

        // This method subscribes to _setWalletDataEvent and set up on scene.
        public void SetWalletData(List<INFT> data, bool isWalletEquipmentListEmpty = false)
        {
            _walletEquipmentList = data;
            AfterSaveData(isWalletEquipmentListEmpty);
        }

        protected override void RenderData()
        {
            foreach (var itemData in _walletEquipmentList)
            {
                var item = Instantiate(_singleItemPrefab, _scrollRect.content).GetComponent<UITransferItem>();
                item.ConfigureCell(itemData);
                SetParentIdentity(item);
            }
        }
    }
}
