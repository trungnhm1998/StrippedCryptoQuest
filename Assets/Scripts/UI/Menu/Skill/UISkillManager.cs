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
        [SerializeField] private MenuSelectionHandler _selectionHandler;
        [SerializeField] private List<UISkillTabButton> _tabSkillButton;
        [SerializeField] private List<UISkillCharacterPanel> _listSkills;
        [SerializeField] private List<Image> _listHighlightBackground;
        private Dictionary<ECharacterClass, UISkillCharacterPanel> _cachedSkills = new();
        private Dictionary<ECharacterClass, UISkillTabButton> _cachedTabButtons = new();
        private UISkillCharacterPanel _currentActivePanel;
        private bool _isSelectedMenu;

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
        private int _currentSelectedTabIndex = 0;
        private readonly List<ECharacterClass> CYCLE_TYPES= new ()
        {
            ECharacterClass.MainCharacter,
            ECharacterClass.Archer,
            ECharacterClass.Priest,
            ECharacterClass.Hero
        };
    }
}
