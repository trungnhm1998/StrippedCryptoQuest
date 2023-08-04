using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Skill;
using CryptoQuest.Gameplay.Skill.ScriptableObjects;
using CryptoQuest.Menu;
using PolyAndCode.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkillCharacterPanel : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private SkillsMockupSO _listSkillMockup;
        [SerializeField] private RecyclableScrollRect _recyclableScrollRect;
        [SerializeField] private AutoScrollRect _autoScrollRect;
        [SerializeField] private GameObject _upHint;
        [SerializeField] private GameObject _downHint;
        [SerializeField] private RectTransform _currentRectTransform;
        [SerializeField] private RectTransform _parentRectTransform;
        [SerializeField] private RectTransform _skillRectTransform;
        [SerializeField] private LocalizeStringEvent _localizeDescription;
        [SerializeField] private Image _tabImage;
        [NonSerialized] public UISkillAbility CurrentSkillSelected;
        [field: SerializeField] public ECharacterClass Character { get; private set; }
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
                if (type != Character) continue;
                _listSkills.Add(new SkillInformation(skillSO));
            }
        }

        private void ShowScrollHints()
        {
            _upHint.SetActive(ShouldMoveUp);
            _downHint.SetActive(ShouldMoveDown);
        }

        private bool ShouldMoveUp => _currentRectTransform.anchoredPosition.y > _skillRectTransform.rect.height;

        private bool ShouldMoveDown =>
                _currentRectTransform.rect.height - _currentRectTransform.anchoredPosition.y
                > _parentRectTransform.rect.height + _skillRectTransform.rect.height / 2;

        private void SelectSkillHandle()
        {
            _autoScrollRect.UpdateScrollRectTransform();
            ShowScrollHints();
            if (EventSystem.current.currentSelectedGameObject.TryGetComponent<UISkillAbility>(out var currentSelectedSkill))
            {
                _localizeDescription.StringReference = currentSelectedSkill.Description;
            }
        }

        public void Deselect()
        {
            _content.SetActive(false);
            ActiveSkillSelection(false);
        }

        public void Select()
        {
            _content.SetActive(true);
            ShowScrollHints();
            ActiveSkillSelection(true);
            if (_listSkillButton.Count > 0)
            {
                _listSkillButton[0].Select();
                SelectSkillHandle();
            }
        }

        public void ActiveSkillSelection(bool isActivated)
        {
            foreach (var button in _listSkillButton)
            {
                _tabImage.enabled = isActivated;
                button.GetComponent<MultiInputButton>().enabled = isActivated;
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
