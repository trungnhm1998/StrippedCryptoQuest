using System.Collections.Generic;
using CryptoQuest.Data.Item;
using CryptoQuest.Events.Gameplay;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Input;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.EventSystems;
using EItemType = CryptoQuest.Gameplay.Inventory.ScriptableObjects.EItemType;

namespace CryptoQuest.UI.Inventory
{
    public class UIInventory : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private MenuSelectionHandler _selectionHandler;
        [SerializeField] private List<UIInventoryTabButton> _tabInventoryButton;
        [SerializeField] private List<UIInventoryPanel> _inventories;
        [SerializeField] private ItemEventChannelSO _OnEquipItemEvent;
        [SerializeField] private List<UsableTypeSO> _cycleTypes;
        private Dictionary<UsableTypeSO, UIInventoryPanel> _cachedInventories;
        private Dictionary<UsableTypeSO, UIInventoryTabButton> _cachedTabButtons;
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
            _inputMediator.MenuSubmitEvent += HandleItemPressed;
            for (int i = 0; i < _tabInventoryButton.Count; i++)
            {
                _tabInventoryButton[i].Clicked += SelectTab;
            }

            _selectionHandler.UpdateDefault(_tabInventoryButton[0].gameObject);
            SelectTab(_cycleTypes[0]);
        }

        private void OnDisable()
        {
            _inputMediator.NextSelectionMenu -= SelectNextMenu;
            _inputMediator.PreviousSelectionMenu -= SelectPreviousMenu;
            _inputMediator.MenuSubmitEvent -= HandleItemPressed;
            for (int i = 0; i < _tabInventoryButton.Count; i++)
            {
                _tabInventoryButton[i].Clicked -= SelectTab;
            }
        }

        private int _currentSelectedTabIndex = 0;

        private void SelectNextMenu()
        {
            _currentSelectedTabIndex++;
            _currentSelectedTabIndex =
                _currentSelectedTabIndex >= _tabInventoryButton.Count ? 0 : _currentSelectedTabIndex;
            SelectTab(_cycleTypes[_currentSelectedTabIndex]);
        }

        private void SelectPreviousMenu()
        {
            _currentSelectedTabIndex--;
            _currentSelectedTabIndex =
                _currentSelectedTabIndex < 0 ? _tabInventoryButton.Count - 1 : _currentSelectedTabIndex;
            SelectTab(_cycleTypes[_currentSelectedTabIndex]);
        }

        private void HandleItemPressed()
        {
            Debug.Log(
                $"{EventSystem.current.currentSelectedGameObject.GetComponent<UIItemInventory>().ItemBase.ItemSO.DisplayName} Pressed!");
            UsableInformation selectedItemBaseItem =
                EventSystem.current.currentSelectedGameObject.GetComponent<UIItemInventory>().ItemBase;
            _OnEquipItemEvent.RaiseEvent(selectedItemBaseItem);
        }

        private void SelectTab(UsableTypeSO type)
        {
            _cachedTabButtons[type].Select();
            _currentActivePanel.Deselect();
            _currentActivePanel = _cachedInventories[type];
            _currentActivePanel.Select();
        }
    }
}