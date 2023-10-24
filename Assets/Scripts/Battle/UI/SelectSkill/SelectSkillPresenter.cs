using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Input;
using CryptoQuest.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Battle.UI.SelectSkill
{
    public class SelectSkillPresenter : MonoBehaviour
    {
        public delegate void SkillTargetTypeDelegate(UISkill skill);

        public SkillTargetTypeDelegate SelectSelfCallback { get; set; }
        private void OnTargetSelf (CastSkillAbility skill) => SelectSelfCallback?.Invoke(_selectedSkill);

        public SkillTargetTypeDelegate SelectSingleEnemyCallback { get; set; }
        private void OnSingleEnemy(CastSkillAbility skill) => SelectSingleEnemyCallback?.Invoke(_selectedSkill);

        public SkillTargetTypeDelegate SelectSingleHeroCallback { get; set; }
        private void OnSingleHero(CastSkillAbility skill) => SelectSingleHeroCallback?.Invoke(_selectedSkill);

        public SkillTargetTypeDelegate SelectAllHeroCallback { get; set; }
        private void OnSelectAllHero(CastSkillAbility skill) => SelectAllHeroCallback?.Invoke(_selectedSkill);

        public SkillTargetTypeDelegate SelectAllEnemyCallback { get; set; }
        private void OnSelectAllEnemy(CastSkillAbility skill) => SelectAllEnemyCallback?.Invoke(_selectedSkill);

        public SkillTargetTypeDelegate SelectEnemyGroupCallback { get; set; }
        private void OnSelectEnemyGroup(CastSkillAbility skill) => SelectEnemyGroupCallback?.Invoke(_selectedSkill);

        [SerializeField] private BattleInput _battleInput;
        [SerializeField] private VerticalButtonSelector _buttonSelector;
        [SerializeField] private AutoScroll _autoScroll;
        [SerializeField] private ScrollRect _skillList;
        [SerializeField] private UISkill _skillPrefab;

        [SerializeField, Header("State event context")]
        private SkillTargetType _singleHeroChannel;

        [SerializeField] private SkillTargetType _singleEnemyChannel;
        [SerializeField] private SkillTargetType _allHeroChannel;
        [SerializeField] private SkillTargetType _allEnemyChannel;
        [SerializeField] private SkillTargetType _enemyGroupChannel;
        [SerializeField] private SkillTargetType _targetSelfChannel;

        private HeroBehaviour _hero;
        private readonly List<UISkill> _skills = new List<UISkill>();

        public void Show(HeroBehaviour hero, bool interactable = true)
        {
            CreateSkillButtonsDifferentHero(hero);
            _skillList.gameObject.SetActive(true);
            RegisterEvents();
            if (interactable)
                _buttonSelector.SelectFirstButton();
            _buttonSelector.Interactable = interactable;
        }

        private void RegisterEvents()
        {
            _battleInput.NavigateEvent += UpdateAutoScroll;
            _singleHeroChannel.EventRaised += OnSingleHero;
            _singleEnemyChannel.EventRaised += OnSingleEnemy;
            _allHeroChannel.EventRaised += OnSelectAllHero;
            _allEnemyChannel.EventRaised += OnSelectAllEnemy;
            _enemyGroupChannel.EventRaised += OnSelectEnemyGroup;
            _targetSelfChannel.EventRaised += OnTargetSelf;
        }

        private void UnregisterEvents()
        {
            _battleInput.NavigateEvent -= UpdateAutoScroll;
            _singleHeroChannel.EventRaised -= OnSingleHero;
            _singleEnemyChannel.EventRaised -= OnSingleEnemy;
            _allHeroChannel.EventRaised -= OnSelectAllHero;
            _allEnemyChannel.EventRaised -= OnSelectAllEnemy;
            _enemyGroupChannel.EventRaised -= OnSelectEnemyGroup;
            _targetSelfChannel.EventRaised -= OnTargetSelf;
        }

        private void CreateSkillButtonsDifferentHero(HeroBehaviour hero)
        {
            if (hero == _hero) return;
            _hero = hero;
            DestroyAllSkillButtons();
            hero.TryGetComponent(out HeroSkills skills);
            foreach (var skill in skills.Skills)
            {
                var skillUI = Instantiate(_skillPrefab, _skillList.content);
                skillUI.Init(skill);
                skillUI.Selected += SelectingTarget;
                _skills.Add(skillUI);
            }
        }

        public void Hide()
        {
            _buttonSelector.Interactable = false;
            _skillList.gameObject.SetActive(false);
            UnregisterEvents();
        }

        private void DestroyAllSkillButtons()
        {
            foreach (var skill in _skills)
            {
                if (skill == null) continue; // TODO: WHY SKILL == NULL?
                skill.Selected -= SelectingTarget;
                Destroy(skill.gameObject);
            }

            _skills.Clear();
        }

        private void OnDisable()
        {
            DestroyAllSkillButtons();
            UnregisterEvents();
        }

        private UISkill _selectedSkill;

        private void SelectingTarget(UISkill skillUI)
        {
            _selectedSkill = skillUI;
            skillUI.Skill.TargetType.RaiseEvent(skillUI.Skill);
        }

        private void UpdateAutoScroll(Vector2 direction)
        {
            _autoScroll.Scroll();
        }
    }
}