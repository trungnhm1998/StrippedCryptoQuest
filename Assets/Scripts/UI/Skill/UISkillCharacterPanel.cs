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
        [SerializeField] private ECharacterSkill _character;
        private List<MultiInputButton> _buttonList = new();
        public ECharacterSkill Character => _character;
        private List<SkillInformation> _listSkills = new List<SkillInformation>();

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
                var type = skillSO.CharacterSkills;
                if (type != _character) continue;
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
            _buttonList.Add(skill.GetComponent<MultiInputButton>());
        }
        #endregion
    }
}
