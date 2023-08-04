using System.Collections.Generic;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkillManager : MonoBehaviour
    {
        [SerializeField] private MenuSelectionHandler _selectionHandler;
        [SerializeField] private List<UISkillCharacterPanel> _listSkills;
        [SerializeField] private List<Image> _listHighlightBackground;
        private Dictionary<ECharacterClass, UISkillCharacterPanel> _cachedSkills = new();
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
