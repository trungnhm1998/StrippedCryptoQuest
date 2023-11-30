using System.Collections.Generic;
using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using IndiGames.Core.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Skill.UI
{
    public class UISkillList : MonoBehaviour
    {
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

        private float _verticalOffset;

        public bool Interactable
        {
            set
            {
                foreach (var skillUi in _skillUIs) skillUi.Interactable = value;
            }
        }

        private void OnEnable()
        {
            TryShowSkillForHero(ServiceProvider.GetService<IPartyController>().Slots[0]
                .HeroBehaviour); // First slot should never be null/empty
            UISkill.InspectingSkillEvent += CacheInspectingSkill;
        }

        private void OnDisable()
        {
            UISkill.InspectingSkillEvent -= CacheInspectingSkill;
        }

        private void CacheInspectingSkill(UISkill skill)
        {
            _skill = skill;
            _lastSelectedSkill = skill.gameObject;
        }

        public bool TryShowSkillForHero(HeroBehaviour hero)
        {
            _lastSelectedSkill = null;
            if (!hero.TryGetComponent(out HeroSkills skillsComponent)) return false;
            var skills = skillsComponent.Skills;

            CleanUpScrollView();
            InitSkillList(skills);
            return skills.Count > 0;
        }

        private void InitSkillList(IReadOnlyList<CastSkillAbility> skills)
        {
            _lastSelectedSkill = null;
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

        private GameObject _lastSelectedSkill;
        private UISkill _skill;
        public UISkill InspectingSkillUI => _skill;

        public void SelectLastSelectedOrFirstSkill()
        {
            if (_skillUIs.Count == 0) return;
            var skillToSelect = _skillUIs[0].gameObject;

            if (_lastSelectedSkill != null)
            {
                skillToSelect = _lastSelectedSkill;
                _lastSelectedSkill = null;
            }

            EventSystem.current.SetSelectedGameObject(skillToSelect);
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