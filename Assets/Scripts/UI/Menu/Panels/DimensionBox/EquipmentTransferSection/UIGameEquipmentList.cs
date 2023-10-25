using System.Collections;
using System.Collections.Generic;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public class UIGameEquipmentList : UIEquipmentList
    {
        private List<IGame> _gameEquipmentList = new List<IGame>();

        // This method subscribes to _setGameDataEvent and set up on scene.
        public void SetGameData(List<IGame> data, bool isGameEquipmentListEmpty = false)
        {
            _gameEquipmentList = data;
            AfterSaveData(isGameEquipmentListEmpty);
        }

        protected override void RenderData()
        {
            foreach (var itemData in _gameEquipmentList)
            {
                var item = Instantiate(_singleItemPrefab, _scrollRectContent).GetComponent<UITransferItem>();
                item.ConfigureCell(itemData);
                SetParentIdentity(item);
            }
        }
    }
}
