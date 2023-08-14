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
        public static event Action<UsableInfo> Using;
        public delegate void InspectingItem(UIConsumableItem item);

        public event InspectingItem Inspecting;
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private Text _quantity;
        [SerializeField] private MultiInputButton _button;
        [SerializeField] private GameObject _selectedBackground;
        private UsableInfo _itemDef;
        public UsableInfo ItemDef => _itemDef;

        private void OnEnable()
        {
            _button.onClick.AddListener(Use);
            _button.Selected += OnInspectingItem;
            _button.Deselected += DeselectButton;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Use);
            _button.Selected -= OnInspectingItem;
            _button.Deselected -= DeselectButton;
        }

        private void Use()
        {
            Using?.Invoke(_itemDef);
        }

        public void Deselect()
        {
            DeselectButton();
        }

        private void DeselectButton()
        {
            _selectedBackground.SetActive(false);
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