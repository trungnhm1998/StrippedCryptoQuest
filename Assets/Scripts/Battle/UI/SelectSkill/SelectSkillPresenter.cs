using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Ability;
using CryptoQuest.Input;
using CryptoQuest.UI.Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Battle.UI.SelectSkill
{
    public class SelectSkillPresenter : MonoBehaviour
    {
        public delegate void SkillTargetTypeDelegate(UISkill skill);

        [SerializeField] private AutoScroll _autoScroll;
        [SerializeField] private BattleInputSO _input;
        [SerializeField] private ScrollRect _skillList;
        [SerializeField] private UISkill _skillPrefab;

        [SerializeField, Header("State event context")]
        private SkillTargetType _singleHeroChannel;
        [SerializeField] private SkillTargetType _singleEnemyChannel;

        private HeroBehaviour _hero;
        private UISkill _lastSelectedSkill;
        private readonly List<UISkill> _skills = new List<UISkill>();

        public void Show(HeroBehaviour hero)
        {
            CreateSkillButtonsDifferentHero(hero);
            DOVirtual.DelayedCall(0.1f, SelectFirstOrLastSelectedSkill);
            _skillList.gameObject.SetActive(true);
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            _input.NavigateEvent += UpdateAutoScroll;
            _singleHeroChannel.EventRaised += OnSingleHero;
            _singleEnemyChannel.EventRaised += OnSingleEnemy;
        }

        private void UnregisterEvents()
        {
            _input.NavigateEvent -= UpdateAutoScroll;
            _singleHeroChannel.EventRaised -= OnSingleHero;
            _singleEnemyChannel.EventRaised -= OnSingleEnemy;
        }

        private void SelectFirstOrLastSelectedSkill()
        {
            if (_lastSelectedSkill != null)
            {
                EventSystem.current.SetSelectedGameObject(_lastSelectedSkill.gameObject);
                _lastSelectedSkill = null;
            }
            else
                EventSystem.current.SetSelectedGameObject(_skillList.content.GetChild(0).gameObject);
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
            _skillList.gameObject.SetActive(false);
            UnregisterEvents();
        }

        public SkillTargetTypeDelegate SelectSingleEnemyCallback { get; set; }
        private void OnSingleEnemy(CastableAbility skill) => SelectSingleEnemyCallback?.Invoke(_lastSelectedSkill);

        public SkillTargetTypeDelegate SelectSingleHeroCallback { get; set; }
        private void OnSingleHero(CastableAbility skill) => SelectSingleHeroCallback?.Invoke(_lastSelectedSkill);

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

        private void SelectingTarget(UISkill skillUI)
        {
            _lastSelectedSkill = skillUI;
            skillUI.Skill.TargetType.RaiseEvent(skillUI.Skill);
        }

        private void UpdateAutoScroll(Vector2 direction)
        {
            _autoScroll.Scroll();
        }
    }
}