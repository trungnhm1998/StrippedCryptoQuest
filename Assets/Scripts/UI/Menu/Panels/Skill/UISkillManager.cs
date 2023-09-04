using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Menu;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkillManager : MonoBehaviour
    {
        public event Action CharacterSelected;

        [SerializeField] private List<UISkillCharacterButton> _tabSkillButton;
        [SerializeField] private List<UISkillCharacterPanel> _listSkills;
        [SerializeField] private List<MultiInputButton> _listCharacterCardButton;
        private Dictionary<ECharacterClass, UISkillCharacterPanel> _cachedSkills = new();
        private Dictionary<ECharacterClass, UISkillCharacterButton> _cachedTabButtons = new();
        private UISkillCharacterPanel _currentActivePanel;
        private bool _isSelectedMenu = false;
        private bool _isSelectedCharacter = false;
        [SerializeField] private int _currentSelectedTabIndex = 0;
        [SerializeField] private int _currentCharacterCardIndex = 0;

        private GameObject _selectedCharacter;

        [Header("Configs")]
        [SerializeField] private ServiceProvider _provider;
        private IParty _playerParty;

        private void Awake()
        {
            InitListSkills();
            _playerParty = _provider.PartyController.Party;
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
            for (int i = 0; i < _tabSkillButton.Count; i++)
            {
                _tabSkillButton[i].Clicked += SelectTab;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _tabSkillButton.Count; i++)
            {
                _tabSkillButton[i].Clicked -= SelectTab;
            }
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
            for (int i = 0; i < _tabSkillButton.Count; i++)
            {
                if (_isSelectedMenu)
                {
                    if (_selectedCharacter == _listCharacterCardButton[i].gameObject)
                        _currentCharacterCardIndex = i;
                }
                else
                {
                    _listSkills[_currentSelectedTabIndex].CharacterCardBackground.enabled = false;
                    if (_selectedCharacter == _tabSkillButton[i].gameObject)
                        _currentSelectedTabIndex = i;
                }
            }
        }

        public void OnClickCharacterCard()
        {
            _selectedCharacter = EventSystem.current.currentSelectedGameObject;
            CharacterSelected?.Invoke();
            DisableAllCharacterButtons();
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

        #region Character Selection
        private void EnableAllCharacterButtons()
        {
            foreach (var button in _tabSkillButton)
            {
                button.enabled = true;
            }
        }
        
        private void DisableAllCharacterButtons()
        {
            foreach (var button in _tabSkillButton)
            {
                button.enabled = false;
            }
        }

        public void InitCharacterSelection()
        {
            EnableAllCharacterButtons();
            _tabSkillButton[0].Select();
        }

        public void DeInitCharacterSelection()
        {
            DisableAllCharacterButtons();
        }
        #endregion

        #region Skill Selection
        public void OnTurnBack()
        {
            BackToSelectCharacterCard();
            SelectCharacterMenu(Vector2.zero);
        }
        #endregion
    }
}