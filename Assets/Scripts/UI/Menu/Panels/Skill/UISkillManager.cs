using System;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.Menu;
using CryptoQuest.UI.Menu.Panels.Status;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkillManager : MonoBehaviour
    {
        public event Action CharacterSelected;

        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private MenuSelectionHandler _selectionHandler;
        [SerializeField] private List<UISkillTabButton> _tabSkillButton;
        [SerializeField] private List<UISkillCharacterPanel> _listSkills;
        [SerializeField] private List<MultiInputButton> _listCharacterCardButton;
        private Dictionary<ECharacterClass, UISkillCharacterPanel> _cachedSkills = new();
        private Dictionary<ECharacterClass, UISkillTabButton> _cachedTabButtons = new();
        private UISkillCharacterPanel _currentActivePanel;
        private bool _isSelectedMenu = false;
        private bool _isSelectedCharacter = false;
        [SerializeField] private int _currentSelectedTabIndex = 0;
        [SerializeField] private int _currentCharacterCardIndex = 0;

        private void Awake()
        {
            InitListSkills();
        }

        private void InitListSkills()
        {
            _currentActivePanel = _listSkills[0];
            for (var i = 0; i < _listSkills.Count; i++)
            {
                var skill = _listSkills[i];
                _cachedSkills.Add(skill.Character, skill);
                _cachedTabButtons.Add(skill.Character, _tabSkillButton[i]);
            }
        }

        private void OnEnable()
        {
            // _inputMediator.EnableMenuInput();
            // _inputMediator.MenuNavigateEvent += SelectCharacterMenu;
            // _inputMediator.MenuSubmitEvent += ShowCharacterSkills;
            // _inputMediator.NextSelectionMenu += SelectNextMenu;
            // _inputMediator.PreviousSelectionMenu += SelectPreviousMenu;
            // _inputMediator.MenuCancelEvent += BackToSelectCharacterCard;
            for (int i = 0; i < _tabSkillButton.Count; i++)
            {
                _tabSkillButton[i].Clicked += SelectTab;
            }

            // _selectionHandler.UpdateDefault(_tabSkillButton[0].gameObject);
            //  SelectTab(ECharacterClass.MainCharacter);
        }

        private void OnDisable()
        {
            // _inputMediator.MenuSubmitEvent -= ShowCharacterSkills;
            // _inputMediator.MenuNavigateEvent -= SelectCharacterMenu;
            // _inputMediator.NextSelectionMenu -= SelectNextMenu;
            // _inputMediator.PreviousSelectionMenu -= SelectPreviousMenu;
            // _inputMediator.MenuCancelEvent -= BackToSelectCharacterCard;
            for (int i = 0; i < _tabSkillButton.Count; i++)
            {
                _tabSkillButton[i].Clicked -= SelectTab;
            }
        }

        private void SelectNextMenu()
        {
            _currentSelectedTabIndex = (_currentSelectedTabIndex + 1) % _tabSkillButton.Count;
            SelectTab(CYCLE_TYPES[_currentSelectedTabIndex]);
        }

        private void SelectPreviousMenu()
        {
            _currentSelectedTabIndex = (_currentSelectedTabIndex - 1 + _tabSkillButton.Count) % _tabSkillButton.Count;
            SelectTab(CYCLE_TYPES[_currentSelectedTabIndex]);
        }

        private void ShowCharacterSkills()
        {
            if (!_isSelectedMenu)
            {
                foreach (var tab in _tabSkillButton)
                {
                    tab.GetComponent<MultiInputButton>().enabled = false;
                }
                SelectTab(CYCLE_TYPES[_currentSelectedTabIndex]);
                _isSelectedMenu = true;
            }
            else
            {
                HandleSkillPressed();
            }
        }

        private void HandleSkillPressed()
        {
            if (_isSelectedCharacter)
            {
                _isSelectedCharacter = false;
                foreach (var card in _listCharacterCardButton)
                {
                    card.enabled = false;
                }
                SelectTab(CYCLE_TYPES[_currentSelectedTabIndex]);
                //TODO: Implement apply skill to character
                Debug.Log($"Apply skill to Character {CYCLE_TYPES[_currentCharacterCardIndex]}");
            }
            else
            {
                if (_listSkills[_currentSelectedTabIndex].TypeOfSkill == ECharacterSkill.TargetCast)
                {
                    _isSelectedCharacter = true;
                    _listSkills[_currentSelectedTabIndex].ActiveSkillSelection(false);
                    foreach (var card in _listCharacterCardButton)
                    {
                        card.enabled = true;
                    }
                    EventSystem.current.SetSelectedGameObject(_listCharacterCardButton[0].gameObject);
                }
                else
                {
                    //TODO: Implement logic to use skill
                    Debug.Log($"Use self cast-skill!");
                }
            }
        }

        private readonly ECharacterClass[] CYCLE_TYPES = new ECharacterClass[]
        {
            ECharacterClass.MainCharacter,
            ECharacterClass.Archer,
            ECharacterClass.Priest,
            ECharacterClass.Hero
        };

        private void SelectTab(ECharacterClass type)
        {
            if (_cachedTabButtons[type].gameObject != null) _cachedTabButtons[type].Select();
            _currentActivePanel.Deselect();
            _currentActivePanel = _cachedSkills[type];
            _currentActivePanel.Select();
        }

        private void SelectCharacterMenu(Vector2 arg0)
        {
            GameObject currentGameObject = EventSystem.current.currentSelectedGameObject;
            for (int i = 0; i < _tabSkillButton.Count; i++)
            {
                if (_isSelectedMenu)
                {
                    if (currentGameObject == _listCharacterCardButton[i].gameObject)
                        _currentCharacterCardIndex = i;
                }
                else
                {
                    _listSkills[_currentSelectedTabIndex].CharacterCardBackground.enabled = false;
                    if (currentGameObject == _tabSkillButton[i].gameObject)
                        _currentSelectedTabIndex = i;
                }
            }
        }

        public void OnClickCharacterCard()
        {
            SelectCharacterMenu(Vector2.zero);
            ShowCharacterSkills();
        }

        public void BackToSelectCharacterCard()
        {
            if (!_isSelectedCharacter)
            {
                foreach (var tab in _tabSkillButton)
                {
                    tab.GetComponent<MultiInputButton>().enabled = true;
                }
                EventSystem.current.SetSelectedGameObject(_tabSkillButton[_currentSelectedTabIndex].gameObject);
                _isSelectedMenu = false;
                _listSkills[_currentSelectedTabIndex].ActiveSkillSelection(false);
            }
            else
            {
                foreach (var card in _listCharacterCardButton)
                {
                    card.enabled = false;
                }
                _isSelectedCharacter = false;
                SelectTab(CYCLE_TYPES[_currentSelectedTabIndex]);
            }
        }

        private void EnableAllCharacterButtons()
        {
            foreach (var slotButton in _tabSkillButton)
            {
                slotButton.enabled = true;
            }
        }

        private void DisableSkillButtons()
        {
            
        }

        public void InitCharacterSelection()
        {
            EnableAllCharacterButtons();
            EventSystem.current.SetSelectedGameObject(_tabSkillButton[0].gameObject);
        }
    }
}