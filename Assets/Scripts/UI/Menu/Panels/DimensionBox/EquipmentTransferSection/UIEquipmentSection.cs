using System.Collections.Generic;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection.Interfaces;
using PolyAndCode.UI;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.EquipmentTransferSection
{
    public class UIEquipmentSection : UITransferSection, IRecyclableScrollRectDataSource
    {
        [SerializeField] private RecyclableScrollRect _scrollRect;

        private List<IData> _itemList = new List<IData>();

        public void SetData(List<IData> data)
        {
            _itemList = data;
            _scrollRect.Initialize(this);
            Invoke(nameof(SetDefaultSelection), .1f);
        }

        /// <summary>
        /// Must delay this method a bit to let the scroll view finish initializing.
        /// </summary>
        private void SetDefaultSelection()
        {
            var firstButton = _scrollRect.content.GetComponentInChildren<MultiInputButton>();
            firstButton.Select();
        }

        public int GetItemCount()
        {
            return _itemList.Count;
        }

        public void SetCell(ICell cell, int index)
        {
            var item = cell as UITransferItem;
            item.ConfigureCell(_itemList[index]);
        }
    }
}
