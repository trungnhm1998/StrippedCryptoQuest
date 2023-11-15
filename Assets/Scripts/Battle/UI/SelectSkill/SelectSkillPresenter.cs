using System;
using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.Gameplay.Battle.Core.Helper;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;

namespace CryptoQuest.Battle.UI.SelectSkill
{
    public class SelectSkillPresenter : MonoBehaviour
    {
        public delegate void SkillTargetTypeDelegate(CastSkillAbility skill);

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

        [SerializeField] private UICommandDetailPanel _skillListUI;

        [SerializeField, Header("State event context")]
        private SkillTargetType _singleHeroChannel;
        [SerializeField] private SkillTargetType _singleEnemyChannel;
        [SerializeField] private SkillTargetType _allHeroChannel;
        [SerializeField] private SkillTargetType _allEnemyChannel;
        [SerializeField] private SkillTargetType _enemyGroupChannel;
        [SerializeField] private SkillTargetType _targetSelfChannel;

        private HeroBehaviour _hero;
        private readonly List<SkillButtonInfo> _skillInfos = new List<SkillButtonInfo>();
        private CastSkillAbility _selectedSkill;
        public CastSkillAbility SelectedSkill => _selectedSkill;

        public void Show(HeroBehaviour hero, bool interactable = true)
        {
            SetInteractive(interactable);
            ShowSkillListUI(hero);
            SetActiveScroll(true);
            RegisterEvents();
        }
        
        public void SetInteractive(bool value)
        {
            _skillListUI.Interactable = value;
        }

        public void SetActiveScroll(bool value)
        {
            _skillListUI.SetActiveContent(value);
        }

        private void RegisterEvents()
        {
            _singleHeroChannel.EventRaised += OnSingleHero;
            _singleEnemyChannel.EventRaised += OnSingleEnemy;
            _allHeroChannel.EventRaised += OnSelectAllHero;
            _allEnemyChannel.EventRaised += OnSelectAllEnemy;
            _enemyGroupChannel.EventRaised += OnSelectEnemyGroup;
            _targetSelfChannel.EventRaised += OnTargetSelf;
        }

        private void UnregisterEvents()
        {
            _singleHeroChannel.EventRaised -= OnSingleHero;
            _singleEnemyChannel.EventRaised -= OnSingleEnemy;
            _allHeroChannel.EventRaised -= OnSelectAllHero;
            _allEnemyChannel.EventRaised -= OnSelectAllEnemy;
            _enemyGroupChannel.EventRaised -= OnSelectEnemyGroup;
            _targetSelfChannel.EventRaised -= OnTargetSelf;
        }

        private void ShowSkillListUI(HeroBehaviour hero)
        {
            if (hero == _hero)
            {
                SetSelectableCurrentHero();
                return;
            } 

            _hero = hero;
            hero.TryGetComponent(out HeroSkills skills);
            var model = new CommandDetailModel();
            _skillInfos.Clear();

            foreach (var skill in skills.Skills)
            {
                if (!skill.SkillInfo.UsageScenarioSO.HasFlag(EAbilityUsageScenario.Battle))
                    continue;
                var skillButtonInfo = new SkillButtonInfo(skill, ConfirmSelectSkill);
                model.AddInfo(skillButtonInfo);
                _skillInfos.Add(skillButtonInfo);
            }

            _skillListUI.ShowCommandDetail(model);

            SetSelectableCurrentHero();
        }

        private void SetSelectableCurrentHero()
        {
            foreach (var info in _skillInfos)
            {
                var isSelectable = info.Skill.IsCastable(_hero.AbilitySystem);
                info.SetSelectable(isSelectable);
            }
        }

        public void Hide()
        {
            SetInteractive(false);
            SetActiveScroll(false);
            UnregisterEvents();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        private void ConfirmSelectSkill(CastSkillAbility skill)
        {
            _selectedSkill = skill;
            skill.TargetType.RaiseEvent(skill);
        }
    }
}