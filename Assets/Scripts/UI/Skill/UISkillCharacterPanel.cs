using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Skill;
using CryptoQuest.Gameplay.Skill.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.Menu;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Skill
{
    public class UISkillCharacterPanel : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        [SerializeField] private SkillsMockupSO _listSkillMockup;
        [SerializeField] private RecyclableScrollRect _recyclableScrollRect;
        [field: SerializeField] public ECharacterClasses CharacterClass { get; private set; }
        private List<MultiInputButton> _listSkillButton = new();
        private List<SkillInformation> _listSkills = new();

        private void Awake()
        {
            InitData();
            _recyclableScrollRect.DataSource = this;
        }


        private void InitData()
        {
            if (_listSkills != null) _listSkills.Clear();
            foreach (var skill in _listSkillMockup.Skills)
            {
                var skillSO = skill.SkillSO;
                var type = skillSO.CharacterClass;
                if (type != CharacterClass) continue;
                _listSkills.Add(new SkillInformation(skillSO));
            }
        }

        #region PLUGINS
        public int GetItemCount()
        {
            return _listSkills.Count;
        }

        public void SetCell(ICell cell, int index)
        {
            var skill = cell as UISkillAbility;
            skill.Init(_listSkills[index]);
            _listSkillButton.Add(skill.GetComponent<MultiInputButton>());
        }
        #endregion
    }
}
