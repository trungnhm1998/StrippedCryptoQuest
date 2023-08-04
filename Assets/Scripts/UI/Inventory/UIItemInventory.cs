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
        private UsableInformation _itemBase;
        public UsableInformation ItemBase => _itemBase;
        public event UnityAction<UsableInformation> Clicked;

        public void Init(UsableInformation item)
        {
            _itemBase = item;
            Icon.sprite = item.Item.Icon;
            Name.StringReference = item.Item.DisplayName;
            Quantity.text = item.Quantity.ToString();
            Description = item.Item.Description;
        }

        public void OnClicked()
        {
            Clicked?.Invoke(_itemBase);
        }

        public void Select()
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}