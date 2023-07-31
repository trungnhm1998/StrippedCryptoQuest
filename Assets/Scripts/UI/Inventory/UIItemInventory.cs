using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Inventory
{
    public class UIItemInventory : MonoBehaviour, ICell
    {
        public Image Icon;
        public LocalizeStringEvent Name;
        public Text Quantity;
        public LocalizedString Description;
        private ItemInfomation _itemInfo;
        public ItemInfomation ItemInfo => _itemInfo;
        public event UnityAction<ItemSO> Clicked;

        public void Init(ItemInfomation itemInfo)
        {
            _itemInfo = itemInfo;
            Icon.sprite = itemInfo.ItemSO.Icon;
            Name.StringReference = itemInfo.ItemSO.Name;
            Quantity.text = itemInfo.Quantity.ToString();
            Description = itemInfo.ItemSO.Description;
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