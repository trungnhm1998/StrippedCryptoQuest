using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Menu;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public class UIConsumableItem : MonoBehaviour, ICell
    {
        public static event Action<UIConsumableItem> Using;

        public delegate void InspectingItem(UIConsumableItem item);

        public event InspectingItem Inspecting;
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private Text _quantity;
        [SerializeField] private MultiInputButton _button;
        [SerializeField] private GameObject _selectedBackground;
        private UsableInfo _itemDef;
        public UsableInfo ItemDef => _itemDef;

        public bool Interactable
        {
            get => _button.interactable;
            set => _button.interactable = value;
        }

        private void OnEnable()
        {
            _button.Selected += OnInspectingItem;
        }

        private void OnDisable()
        {
            _button.Selected -= OnInspectingItem;
        }

        public void OnUse()
        {
            Using?.Invoke(this);
        }

        public void Use()
        {
            _itemDef.Use();
        }

        private void OnInspectingItem()
        {
            _selectedBackground.SetActive(true);
            Inspecting?.Invoke(this);
        }


        public void Init(UsableInfo item)
        {
            _itemDef = item;
            _icon.sprite = item.Icon;
            _name.StringReference = item.DisplayName;
            _quantity.text = item.Quantity.ToString();
        }

        public void Inspect()
        {
            _button.Select();
            OnInspectingItem();
        }
    }
}