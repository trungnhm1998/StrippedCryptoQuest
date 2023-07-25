using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Events;
using CryptoQuest.Item.Inventory;
using UnityEngine.EventSystems;
using CryptoQuest.Gameplay.Inventory;

namespace CryptoQuest.UI.Inventory
{
    public class UIItemInventory : MonoBehaviour, ICell
    {
        public Image Icon;
        public LocalizeStringEvent Name;
        public Text Quantity;
        private ItemInfomation _itemInfo;

        public event UnityAction<ItemSO> Clicked;

        public void Init(ItemInfomation itemInfo)
        {
            _itemInfo = itemInfo;
            Icon.sprite = itemInfo.ItemSO.Icon;
            Name.StringReference = itemInfo.ItemSO.Name;
            Quantity.text = itemInfo.Quantity.ToString();
        }

        public void OnClicked()
        {
            Clicked?.Invoke(_itemInfo.ItemSO);
        }

        public void Select()
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}