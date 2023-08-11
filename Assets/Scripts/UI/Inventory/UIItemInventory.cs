using UnityEngine;
using UnityEngine.UI;
using PolyAndCode.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Inventory
{
    public class UIItemInventory : MonoBehaviour, ICell
    {
        [SerializeField] private Image Icon;
        [SerializeField] private LocalizeStringEvent Name;
        [SerializeField] private Text Quantity;        
        [field: SerializeField] public LocalizedString Description{get; private set;}
        [field: SerializeField] public UsableInfo ItemBase { get; private set; }

        public void Init(UsableInfo item)
        {
            ItemBase = item;
            Icon.sprite = item.Item.Image;
            Name.StringReference = item.Item.DisplayName;
            Quantity.text = item.Quantity.ToString();
            Description = item.Item.Description;
        }

        public void OnClicked()
        {
            ItemBase.UseItem();
        }

        public void Select()
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}