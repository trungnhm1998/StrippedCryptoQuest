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
        private Dictionary<ECharacterSkill, UISkillCharacterPanel> _cachedSkills = new();
        private Dictionary<ECharacterSkill, UISkillTabButton> _cachedTabButtons = new();
        private UISkillCharacterPanel _currentActivePanel;

        private void Awake()
        {
            InitListSkills();
        }

        private void InitListSkills()
        {
            _currentActivePanel = _listSkills[0];
            _cachedSkills = new();
            _cachedTabButtons = new();

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
            _selectionHandler.UpdateDefault(_tabSkillButton[0].gameObject);
        }
    }
}
