using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using CryptoQuest.Item.Inventory;

namespace CryptoQuest.UI.Inventory
{
    public class UIItemStatus : MonoBehaviour, ICell
    {
        public Image Icon;
        public Text Name;
        public Text Amount;
        private ItemInformation _itemInfo;
        private int _cellIndex;

        public void ConfigureCell(ItemInformation itemInfo, int cellIndex)
        {
            _cellIndex = cellIndex;
            _itemInfo = itemInfo;
            Icon.sprite = itemInfo.Icon;
            Name.text = itemInfo.NameItem;
            Amount.text = itemInfo.Amount.ToString();
        }
    }
}