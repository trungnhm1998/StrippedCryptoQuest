using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Skill
{
    public class UISkillManager : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private MenuSelectionHandler _selectionHandler;
        [SerializeField] private List<UISkillTabButton> _tabSkillButton;
        [SerializeField] private List<UISkillCharacterPanel> _listSkills;
        [SerializeField] private List<Image> _listHighlightBackground;
        private Dictionary<ECharacterClass, UISkillCharacterPanel> _cachedSkills = new();
        private Dictionary<ECharacterClass, UISkillTabButton> _cachedTabButtons = new();
        private UISkillCharacterPanel _currentActivePanel;
        private bool _isSelectedMenu = false;
        private int _currentSelectedTabIndex = 0;

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
            _inputMediator.EnableMenuInput();
            _inputMediator.MenuNavigateEvent += SelectCharacterMenu;
            _inputMediator.MenuSubmitEvent += ShowCharacterSkills;
            _inputMediator.NextSelectionMenu += SelectNextMenu;
            _inputMediator.PreviousSelectionMenu += SelectPreviousMenu;
            _inputMediator.CancelEvent += BackToSelectCharacterCard;
            for (int i = 0; i < _tabSkillButton.Count; i++)
            {
                _tabSkillButton[i].Clicked += SelectTab;
            }

            _selectionHandler.UpdateDefault(_tabSkillButton[0].gameObject);
            SelectTab(ECharacterClass.MainCharacter);
        }

        private void OnDisable()
        {
            _inputMediator.MenuSubmitEvent -= ShowCharacterSkills;
            _inputMediator.MenuNavigateEvent -= SelectCharacterMenu;
            _inputMediator.NextSelectionMenu -= SelectNextMenu;
            _inputMediator.PreviousSelectionMenu -= SelectPreviousMenu;
            _inputMediator.CancelEvent -= BackToSelectCharacterCard;
            for (int i = 0; i < _tabSkillButton.Count; i++)
            {
                _tabSkillButton[i].Clicked -= SelectTab;
            }
        }
        private void SelectNextMenu()
        {
            _listHighlightBackground[_currentSelectedTabIndex].enabled = false;
            _currentSelectedTabIndex = (_currentSelectedTabIndex + 1) % _tabSkillButton.Count;
            SelectTab(CYCLE_TYPES[_currentSelectedTabIndex]);
        }

        private void SelectPreviousMenu()
        {
            _listHighlightBackground[_currentSelectedTabIndex].enabled = false;
            _currentSelectedTabIndex = (_currentSelectedTabIndex - 1 + _tabSkillButton.Count) % _tabSkillButton.Count;
            SelectTab(CYCLE_TYPES[_currentSelectedTabIndex]);
        }


        private void ShowCharacterSkills()
        {
            foreach (var tab in _tabSkillButton)
            {
                tab.GetComponent<MultiInputButton>().enabled = false;
            }
            SelectTab(CYCLE_TYPES[_currentSelectedTabIndex]);
            _isSelectedMenu = true;
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

        private void SelectCharacterMenu()
        {
            for (int i = 0; i < _tabSkillButton.Count; i++)
            {
                if (_tabSkillButton[i].gameObject == EventSystem.current.currentSelectedGameObject)
                {
                    _currentSelectedTabIndex = i;
                    break;
                }
            }
        }

        public void OnClickCharacterCard()
        {
            SelectCharacterMenu();
            ShowCharacterSkills();
        }

        public void BackToSelectCharacterCard()
        {
            foreach (var tab in _tabSkillButton)
            {
                tab.GetComponent<MultiInputButton>().enabled = true;
            }
            EventSystem.current.SetSelectedGameObject(_tabSkillButton[_currentSelectedTabIndex].gameObject);
            _isSelectedMenu = false;
            _listHighlightBackground[_currentSelectedTabIndex].enabled = false;
            _listSkills[_currentSelectedTabIndex].ActiveSkillSelection(false);

        }
    }
}