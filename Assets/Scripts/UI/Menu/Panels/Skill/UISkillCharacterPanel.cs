using System;
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

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkillCharacterPanel : MonoBehaviour, IRecyclableScrollRectDataSource
    {
        [Header("Configs")]
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _singleItemRect;

        [Space]
        public AbilityDataProviderSO AbilityDataProvider;
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private GameObject _content;
        [SerializeField] private RecyclableScrollRect _recyclableScrollRect;
        [SerializeField] private GameObject _upHint;
        [SerializeField] private GameObject _downHint;
        [SerializeField] private RectTransform _currentRectTransform;
        [SerializeField] private RectTransform _parentRectTransform;
        [SerializeField] private RectTransform _skillRectTransform;
        [SerializeField] private LocalizeStringEvent _localizeDescription;
        [field: SerializeField] public ECharacterClass Character { get; private set; }
        private List<MultiInputButton> _listSkillButton = new();
        private List<SkillInformation> _listSkills = new();
        [NonSerialized] public ECharacterSkill TypeOfSkill;
        [NonSerialized] public UISkillAbilityButton CurrentSkillAbilityButton;
        public Image CharacterCardBackground;

        private RectTransform _inventoryViewport;
        private float _verticalOffset;
        private float _lowerBound;
        private float _upperBound;


        private void Awake()
        {
            InitScrollviewInfo();
            InitData();
            _recyclableScrollRect.DataSource = this;
        }

        private void OnEnable()
        {
            _inputMediator.EnableMenuInput();
            _inputMediator.MenuNavigateEvent += SelectSkillHandle;
        }

        private void OnDisable()
        {
            _inputMediator.MenuNavigateEvent -= SelectSkillHandle;
        }

        private void InitScrollviewInfo()
        {
            _verticalOffset = _singleItemRect.rect.height;
            _inventoryViewport = _scrollRect.viewport;
            var position = _inventoryViewport.position;
            var rect = _inventoryViewport.rect;
            _lowerBound = position.y - rect.height / 2;
            _upperBound = position.y + rect.height / 2 + _verticalOffset;
        }

        private void AutoScroll(Button button)
        {
            var selectedRowPositionY = button.transform.position.y;

            if (selectedRowPositionY <= _lowerBound)
            {
                _scrollRect.content.anchoredPosition += Vector2.up * _verticalOffset;
            }
            else if (selectedRowPositionY >= _upperBound)
            {
                _scrollRect.content.anchoredPosition += Vector2.down * _verticalOffset;
            }
        }

        private void InitData()
        {
            if (_listSkills != null) _listSkills.Clear();
            _listSkills = AbilityDataProvider.GetAllAbility();
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

        private void SelectSkillHandle(Vector2 arg0)
        {

            AutoScroll(EventSystem.current.currentSelectedGameObject.GetComponent<Button>());
            ShowScrollHints();
            if (EventSystem.current.currentSelectedGameObject.TryGetComponent<UISkillAbilityButton>(
                    out var currentSelectedSkill))
            {
                CurrentSkillAbilityButton = currentSelectedSkill;
                TypeOfSkill = CurrentSkillAbilityButton.TypeOfSkill;
                _localizeDescription.StringReference = CurrentSkillAbilityButton.Description;
            }
        }

        public void Deselect()
        {
            _content.SetActive(false);
            ActiveSkillSelection(false);
            CharacterCardBackground.enabled = false;
        }

        public void Select()
        {
            _content.SetActive(true);
            ShowScrollHints();
            ActiveSkillSelection(true);
            CharacterCardBackground.enabled = true;
            if (_listSkillButton.Count > 0)
            {
                _listSkillButton[0].Select();
                SelectSkillHandle(Vector2.zero);
            }
        }

        public void ActiveSkillSelection(bool isActivated)
        {
            foreach (var button in _listSkillButton)
            {
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
            var skill = cell as UISkillAbilityButton;
            skill.Init(_listSkills[index]);
            _listSkillButton.Add(skill.GetComponent<MultiInputButton>());
        }

        #endregion
    }
}