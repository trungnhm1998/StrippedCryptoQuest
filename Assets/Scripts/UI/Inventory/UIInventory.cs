using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.Item.Inventory;
using CryptoQuest.Menu;
using UnityEngine;

namespace CryptoQuest.UI.Inventory
{
    public class UIInventory : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private MenuSelectionHandler _selectionHandler;
        [SerializeField] private List<UIInventoryTabButton> _tabInventoryButton;
        [SerializeField] private List<UIInventoryPanel> _inventories;
        private Dictionary<EItemType, UIInventoryPanel> _cachedInventories;
        private Dictionary<EItemType, UIInventoryTabButton> _cachedTabButtons;
        private UIInventoryPanel _currentActivePanel;
        private void Awake()
        {
            InitInventories();
        }

        private void InitInventories()
        {
            _currentActivePanel = _inventories[0];
            _cachedInventories = new();
            _cachedTabButtons = new();

            for (var i = 0; i < _inventories.Count; i++)
            {
                var inventory = _inventories[i];
                _cachedInventories.Add(inventory.Type, inventory);
                _cachedTabButtons.Add(inventory.Type, _tabInventoryButton[i]);
            }
        }

        private void OnEnable()
        {
            _inputMediator.EnableMenuInput();

            _inputMediator.NextSelectionMenu += SelectNextMenu;
            _inputMediator.PreviousSelectionMenu += SelectPreviousMenu;
            _inputMediator.MenuNavigateEvent += SelectItemHandle;
            for (int i = 0; i < _tabInventoryButton.Count; i++)
            {
                _tabInventoryButton[i].Clicked += SelectTab;
            }

            _selectionHandler.UpdateDefault(_tabInventoryButton[0].gameObject);
            SelectTab(EItemType.Expendables);
        }

        private void OnDisable()
        {
            _inputMediator.NextSelectionMenu -= SelectNextMenu;
            _inputMediator.PreviousSelectionMenu -= SelectPreviousMenu;
            _inputMediator.MenuNavigateEvent -= SelectItemHandle;
            for (int i = 0; i < _tabInventoryButton.Count; i++)
            {
                _tabInventoryButton[i].Clicked -= SelectTab;
            }
        }

        private void SelectItemHandle()
        {
        }

        private List<EItemType> _cycleType = new List<EItemType>()
        {
            EItemType.Expendables,
            EItemType.Valuables,
            EItemType.NFT
        };
        private int _currentSelectedTabIndex = 0;

        private void SelectNextMenu()
        {
            _currentSelectedTabIndex++;
            _currentSelectedTabIndex = _currentSelectedTabIndex >= _tabInventoryButton.Count ? 0 : _currentSelectedTabIndex;
            SelectTab(_cycleType[_currentSelectedTabIndex]);
        }

        private void SelectPreviousMenu()
        {
            _currentSelectedTabIndex--;
            _currentSelectedTabIndex = _currentSelectedTabIndex < 0 ? _tabInventoryButton.Count - 1 : _currentSelectedTabIndex;
            SelectTab(_cycleType[_currentSelectedTabIndex]);
        }

        private void SelectTab(EItemType type)
        {
            _cachedTabButtons[type].Select();
            _currentActivePanel.Deselect();
            _currentActivePanel = _cachedInventories[type];
            _currentActivePanel.Select();
        }
    }
}
