using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu.Panels.Skill
{
    public class UISkillList : MonoBehaviour
    {
        public static UnityAction EnterSkillSelectionEvent;

        [SerializeField] private UICharacterSelection _characterSelection;

        [Header("Scroll View Configs")]
        [SerializeField] private ScrollRect _scrollRect;

        [SerializeField] private GameObject _upArrow;
        [SerializeField] private GameObject _downArrow;
        [SerializeField] private UISkill _prefab;
        private IObjectPool<UISkill> _skillUIPool;

        public IObjectPool<UISkill> SkillUIPool =>
            _skillUIPool ??= new ObjectPool<UISkill>(OnCreate, OnGet, OnRelease, OnDestroySkill);

        private List<UISkill> _skillUIs = new();

        private UISkillButton _defaultSelectedSkill;
        private float _verticalOffset;
        private IPartyController _partyController;

        private void Awake()
        {
            _characterSelection.InspectingCharacterEvent += Init;
        }

        private void OnDestroy()
        {
            _characterSelection.InspectingCharacterEvent -= Init;
        }

        private void OnEnable()
        {
            CleanUpScrollView();
            _partyController = ServiceProvider.GetService<IPartyController>();
            Init(_partyController.Slots[0].HeroBehaviour); // First slot should never be null/empty
        }

        private void Init(HeroBehaviour hero)
        {
            if (!hero.TryGetComponent(out HeroSkills skillsComponent)) return;
            var skills = skillsComponent.Skills;

            CleanUpScrollView();
            InitSkillList(skills);
            OnSelectFirstSkill();
        }

        private void InitSkillList(IReadOnlyList<CastSkillAbility> skills)
        {
            foreach (var skill in skills)
            {
                var skillUI = SkillUIPool.Get();
                skillUI.Init(skill);
            }
        }

        private void CleanUpScrollView()
        {
            foreach (var ui in _skillUIs)
            {
                SkillUIPool.Release(ui);
            }
            _skillUIs = new();
        }

        private void OnSelectFirstSkill()
        {
            EnterSkillSelectionEvent?.Invoke();

            _defaultSelectedSkill = _scrollRect.content.GetComponentInChildren<UISkillButton>();
            _defaultSelectedSkill.Select();
        }

        private bool ShouldMoveUp => _scrollRect.content.anchoredPosition.y > _verticalOffset;

        private bool ShouldMoveDown =>
            _scrollRect.content.rect.height - _scrollRect.content.anchoredPosition.y
            > _scrollRect.viewport.rect.height + _verticalOffset / 2;

        private void DisplayNavigateArrows()
        {
            _upArrow.SetActive(ShouldMoveUp);
            _downArrow.SetActive(ShouldMoveDown);
        }

        #region SkillSelectionState Setup

        public void Init()
        {
            OnSelectFirstSkill();
        }

        #endregion

        #region Pool Callbacks

        private void OnDestroySkill(UISkill skillUI)
        {
            Destroy(skillUI);
        }

        private void OnRelease(UISkill skillUI)
        {
            skillUI.gameObject.SetActive(false);
        }

        private void OnGet(UISkill skillUI)
        {
            _skillUIs.Add(skillUI);
            skillUI.transform.SetAsLastSibling();
            skillUI.gameObject.SetActive(true);
        }

        private UISkill OnCreate() => Instantiate(_prefab, _scrollRect.content);

        #endregion
    }
}