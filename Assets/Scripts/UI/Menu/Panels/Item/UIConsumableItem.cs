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
        public delegate void InspectingItem(UsableInfo item);

        public static event InspectingItem Inspecting;
        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _name;
        [SerializeField] private Text _quantity;
        [SerializeField] private MultiInputButton _button;
        [SerializeField] private GameObject _selectedBackground;
        private UsableInfo _consumableItem;

        private void OnEnable()
        {
            _button.Selected += OnInspectingItem;
            _button.Deselected += DeselectButton;
        }

        private void OnDisable()
        {
            _button.Selected -= OnInspectingItem;
            _button.Deselected -= DeselectButton;
        }

        private void DeselectButton()
        {
            _selectedBackground.SetActive(false);
        }

        private void OnInspectingItem()
        {
            _selectedBackground.SetActive(true);
            Inspecting?.Invoke(_consumableItem);
        }


        public void Init(UsableInfo item)
        {
            _consumableItem = item;
            _icon.sprite = item.Icon;
            _name.StringReference = item.DisplayName;
            _quantity.text = item.Quantity.ToString();
        }

        public void Inspect()
        {
            OnInspectingItem();
        }
    }
}