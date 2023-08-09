using System.Collections.Generic;
using CryptoQuest.Events.Gameplay;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Input;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Inventory
{
    public class UIInventory : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private MenuSelectionHandler _selectionHandler;
        [SerializeField] private ItemEventChannelSO _OnEquipItemEvent;
        [SerializeField] private UsableActiveTypeSO _usableActiveType;
        [SerializeField] private List<UIInventoryTabButton> _tabInventoryButtons;
        [SerializeField] private List<UIInventoryPanel> _inventoryPanels;
        [SerializeField] private List<MultiInputButton> _characterCardButtons;
        [SerializeField] private List<UsableTypeSO> _cycleTypes;

        private Dictionary<UsableTypeSO, UIInventoryPanel> _cachedInventories = new();
        private Dictionary<UsableTypeSO, UIInventoryTabButton> _cachedTabButtons = new();
        private UIInventoryPanel _currentActivePanel;
        private UIItemInventory _selectedItem;
        private bool _isSelectedMenu = false;
        private bool _isSelectedCharacter = false;
        private int _currentSelectedTabIndex = 0;
        private int _currentCharacterCardIndex = 0;

        private void Awake()
        {
            InitializeInventories();
        }
        private void OnEnable()
        {
            InitializeInputMediator();
            InitializeTabButtonEvents();
            _selectionHandler.UpdateDefault(_tabInventoryButtons[0].gameObject);
            SelectTab(_cycleTypes[0]);
        }

        private void OnDisable()
        {
            UnsubscribeInputMediatorEvents();
            UnsubscribeTabButtonEvents();
        }

        private void InitializeInventories()
        {
            _currentActivePanel = _inventoryPanels[0];
            for (int i = 0; i < _inventoryPanels.Count; i++)
            {
                var inventory = _inventoryPanels[i];
                _cachedInventories.Add(inventory.Type, inventory);
                _cachedTabButtons.Add(inventory.Type, _tabInventoryButtons[i]);
            }
        }


        private void InitializeInputMediator()
        {
            _inputMediator.EnableMenuInput();
            _inputMediator.MenuNavigateEvent += SelectCharacterMenu;
            _inputMediator.NextSelectionMenu += SelectNextMenu;
            _inputMediator.PreviousSelectionMenu += SelectPreviousMenu;
            _inputMediator.MenuSubmitEvent += HandleMenuPressed;
            _inputMediator.MenuCancelEvent += BackToSelectMenu;
        }

        private void InitializeTabButtonEvents()
        {
            foreach (var tabButton in _tabInventoryButtons)
            {
                tabButton.Clicked += SelectTab;
            }
        }

        private void UnsubscribeInputMediatorEvents()
        {
            _inputMediator.MenuNavigateEvent -= SelectCharacterMenu;
            _inputMediator.NextSelectionMenu -= SelectNextMenu;
            _inputMediator.PreviousSelectionMenu -= SelectPreviousMenu;
            _inputMediator.MenuSubmitEvent -= HandleMenuPressed;
            _inputMediator.MenuCancelEvent -= BackToSelectMenu;
        }

        private void UnsubscribeTabButtonEvents()
        {
            foreach (var tabButton in _tabInventoryButtons)
            {
                tabButton.Clicked -= SelectTab;
            }
        }

        private void SelectNextMenu()
        {
            _currentSelectedTabIndex = (_currentSelectedTabIndex + 1) % _tabInventoryButtons.Count;
            SelectTab(_cycleTypes[_currentSelectedTabIndex]);
        }

        private void SelectPreviousMenu()
        {
            _currentSelectedTabIndex = (_currentSelectedTabIndex - 1 + _tabInventoryButtons.Count) % _tabInventoryButtons.Count;
            SelectTab(_cycleTypes[_currentSelectedTabIndex]);
        }

        private void HandleMenuPressed()
        {
            if (!_isSelectedMenu)
            {
                DisableTabButtonInput();
                SelectTab(_cycleTypes[_currentSelectedTabIndex]);
                _isSelectedMenu = true;
                return;
            }
            HandleItemPressed();
        }

        private void DisableTabButtonInput()
        {
            foreach (var tabButton in _tabInventoryButtons)
            {
                tabButton.GetComponent<MultiInputButton>().enabled = false;
            }
        }
        private void HandleItemPressed()
        {
            if (_isSelectedCharacter)
                HandleSelectingCharacterState();
            else
                HandleNormalState();
        }

        private void HandleNormalState()
        {
            _selectedItem = EventSystem.current.currentSelectedGameObject.GetComponent<UIItemInventory>();
            if (_selectedItem.ItemBase.Item.Type != _usableActiveType) return;
            _isSelectedCharacter = true;
            _inventoryPanels[_currentSelectedTabIndex].ActiveItemSelection(false);
            foreach (var cardButton in _characterCardButtons)
            {
                cardButton.enabled = true;
            }
            _currentCharacterCardIndex = 0;
            EventSystem.current.SetSelectedGameObject(_characterCardButtons[0].gameObject);
        }

        private void HandleSelectingCharacterState()
        {
            _isSelectedCharacter = false;
            foreach (var cardButton in _characterCardButtons)
            {
                cardButton.enabled = false;
            }
            SelectTab(_cycleTypes[_currentSelectedTabIndex]);
            Debug.Log($"Apply item {_selectedItem.ItemBase.Item.name} to Character {_currentCharacterCardIndex}");
        }

        private void SelectTab(UsableTypeSO type)
        {
            _cachedTabButtons[type].Select();
            _currentActivePanel.Deselect();
            _currentActivePanel = _cachedInventories[type];
            _currentActivePanel.Select();
        }

        public void BackToSelectMenu()
        {
            if (!_isSelectedCharacter)
            {
                EnableTabButtonInput();
                return;
            }
            DisableCharacterCardButtons();

        }

        private void EnableTabButtonInput()
        {
            foreach (var tabButton in _tabInventoryButtons)
            {
                tabButton.GetComponent<MultiInputButton>().enabled = true;
            }
            _isSelectedMenu = false;
            _inventoryPanels[_currentSelectedTabIndex].ActiveItemSelection(false);
            EventSystem.current.SetSelectedGameObject(_tabInventoryButtons[_currentSelectedTabIndex].gameObject);
        }

        private void DisableCharacterCardButtons()
        {
            _isSelectedCharacter = false;
            SelectTab(_cycleTypes[_currentSelectedTabIndex]);
            foreach (var cardButton in _characterCardButtons)
            {
                cardButton.enabled = false;
            }
        }
        
        private void SelectCharacterMenu(Vector2 arg0)
        {
            GameObject currentGameObject = EventSystem.current.currentSelectedGameObject;
            List<MultiInputButton> _tabInventory = _tabInventoryButtons.ConvertAll(x => x.GetComponent<MultiInputButton>());
            if (_isSelectedMenu)
            {
                _currentCharacterCardIndex = FindIndexInList(currentGameObject, _characterCardButtons);
            }
            else
            {
                _currentSelectedTabIndex = FindIndexInList(currentGameObject, _tabInventory);
                _inventoryPanels[_currentSelectedTabIndex].MenuItem.enabled = false;
            }
        }

        private int FindIndexInList(GameObject targetObject, List<MultiInputButton> buttonList)
        {
            for (int i = 0; i < buttonList.Count; i++)
            {
                if (buttonList[i].gameObject == targetObject)
                {
                    return i;
                }
            }
            return -1; // If not found
        }
    }
}