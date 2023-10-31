using System.Collections.Generic;
using CryptoQuest.Tavern.Interfaces;

namespace CryptoQuest.Tavern.UI.CharacterReplacement
{
    public class UICharacterListWallet : UICharacterList
    {
        private List<IWalletCharacterData> _walletCharacterList = new List<IWalletCharacterData>();

        public void SetWalletData(List<IWalletCharacterData> data, bool isGameEquipmentListEmpty = false)
        {
            _walletCharacterList = data;
            AfterSaveData(isGameEquipmentListEmpty);
        }

        protected override void RenderData()
        {
            foreach (var itemData in _walletCharacterList)
            {
                var item = Instantiate(_singleItemPrefab, _scrollRectContent).GetComponent<UITavernItem>();
                item.SetItemInfo(itemData);
                SetParentIdentity(item);
            }
        }
    }
}