using System.Collections.Generic;
using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.ScriptableObjects;
using CryptoQuest.Battle.UI.Logs;
using CryptoQuest.Gameplay;
using CryptoQuest.Input;
using CryptoQuest.Menus.Skill.UI;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Skill.States
{
    public class SkillSelectionState : SkillStateBase
    {
        private readonly UISkillList _skillListPanel;
        private readonly InputMediatorSO _input;
        private IScenarioChecker _scenarioChecker;

        public SkillSelectionState(UISkillMenu skillPanel) : base(skillPanel)
        {
            _skillListPanel = _skillPanel.SkillListPanel;
            _input = _skillPanel.Input;
        }

        public override void OnEnter()
        {
            _skillPanel.SingleAlliedTarget.EventRaised += SelectSingleAlly;
            _skillPanel.AllAlliesTarget.EventRaised += SelectAllHeroes;
            _skillPanel.SelfTarget.EventRaised += SelectSelf;

            _skillPanel.SelectingHero.EnableSelectBackground();

            _skillPanel.Input.MenuConfirmedEvent += OnCastSkill;
            _scenarioChecker = ServiceProvider.GetService<IScenarioChecker>();
            _input.MenuCancelEvent += HandleCancel;

            ShowSkillList();
        }

        private void ShowSkillList()
        {
            var showSuccess = _skillListPanel.TryShowSkillForHero(_skillPanel.SelectingHero.Hero);
            if (!showSuccess)
            {
                // If this character dont have any skill then back to select character
                HandleCancel();
                return;
            }

            _skillListPanel.Interactable = true;
            _skillListPanel.SelectLastSelectedOrFirstSkill();
        }

        private void OnCastSkill()
        {
            var castSkillAbility = _skillListPanel.InspectingSkillUI.Skill;
            bool isAllowed = _scenarioChecker.IsCorrectScenario(castSkillAbility.SkillInfo.UsageScenarioSO);
            if (!isAllowed)
                return;

            _skillPanel.Input.MenuConfirmedEvent -= OnCastSkill;

            _skillPanel.EnableHeroSelectedMode();

            castSkillAbility.TargetType.RaiseEvent(castSkillAbility);
        }

        public override void OnExit()
        {
            _skillPanel.Input.MenuConfirmedEvent -= OnCastSkill;
            _skillPanel.SingleAlliedTarget.EventRaised -= SelectSingleAlly;
            _skillPanel.AllAlliesTarget.EventRaised -= SelectAllHeroes;
            _skillPanel.SelfTarget.EventRaised -= SelectSelf;

            _input.MenuConfirmedEvent -= SetTargetAsCurrentSelectGameObjectAndCast;
            _input.MenuConfirmedEvent -= CastSkill;
            _input.MenuCancelEvent -= HandleCancel;
            _skillListPanel.Interactable = false;
        }

        private CastSkillAbility _selectingSkill;
        private List<HeroBehaviour> _targets;

        private void SelectSelf(CastSkillAbility skill)
        {
            if (!_skillPanel.SelectingHero.Hero.IsValidAndAlive()) return;
            DisableSkillButtonsAndCacheSelectingSkill(skill);
            _targets = new List<HeroBehaviour> { _skillPanel.SelectingHero.Hero };
            CastSkill();
        }

        // TODO: Move to separate state
        private void SelectAllHeroes(CastSkillAbility skill)
        {
            if (!_skillPanel.SelectingHero.Hero.IsValidAndAlive()) return;
            DisableSkillButtonsAndCacheSelectingSkill(skill);
            _targets = new List<HeroBehaviour>();
            foreach (var hero in _skillPanel.HeroButtons)
            {
                if (hero.Hero != null && hero.Hero.IsValidAndAlive() == false) continue;
                _targets.Add(hero.Hero);
            }

            CastSkill();
        }

        // TODO: Move to separate state
        private void SelectSingleAlly(CastSkillAbility skill)
        {
            if (!_skillPanel.SelectingHero.Hero.IsValidAndAlive()) return;
            DisableSkillButtonsAndCacheSelectingSkill(skill);
            _input.MenuConfirmedEvent -= CastSkill;
            _input.MenuConfirmedEvent += SetTargetAsCurrentSelectGameObjectAndCast;
            _skillPanel.EnableAllHeroButtons();
            _skillPanel.HeroButtons[0].Select();
        }

        // TODO: Move to separate state
        private void SetTargetAsCurrentSelectGameObjectAndCast()
        {
            _input.MenuConfirmedEvent -= SetTargetAsCurrentSelectGameObjectAndCast;
            var eventSystem = EventSystem.current;
            if (eventSystem is null || eventSystem.currentSelectedGameObject == null) return;
            if (eventSystem.currentSelectedGameObject.TryGetComponent(out UISkillCharacterPartySlot slot) ==
                false) return;

            _targets = new List<HeroBehaviour> { slot.Hero };
            CastSkill();
        }

        private void DisableSkillButtonsAndCacheSelectingSkill(CastSkillAbility skill)
        {
            // clear current selected game object
            EventSystem.current.SetSelectedGameObject(null);

            _input.MenuConfirmedEvent += CastSkill;
            DisableAllSkillButtons();
            _selectingSkill = skill;
        }

        private void DisableAllSkillButtons(bool disabled = true) => _skillListPanel.Interactable = !disabled;
        private void DeSelectAllHeroes() => _skillPanel.EnableAllHeroButtons(false);

        private void CastSkill()
        {
            _input.MenuConfirmedEvent -= CastSkill;
            if (_selectingSkill == null || _targets == null)
            {
                ClearTargetSelectionAndSelectingSkill();
                return;
            }

            var systems = from hero in _targets
                where hero != null && hero.IsValid()
                select hero.AbilitySystem;

            var abilitySystemBehaviours = systems as AbilitySystemBehaviour[] ?? systems.ToArray();
            if (abilitySystemBehaviours.Length == 0)
            {
                ClearTargetSelectionAndSelectingSkill();
                return;
            }

            var skillSpec =
                _skillPanel.SelectingHero.Hero.AbilitySystem.GiveAbility<CastSkillAbilitySpec>(_selectingSkill);
            if (!CheckCostRequirement(skillSpec))
            {
                ClearTargetSelectionAndSelectingSkill();
                return;
            }

            skillSpec.Execute(abilitySystemBehaviours);
            ClearTargetSelectionAndSelectingSkill();
        }

        private bool CheckCostRequirement(CastSkillAbilitySpec skillSpec)
        {
            if (skillSpec.CheckCost()) return true;
            ActionDispatcher.Dispatch(new NotEnoughMpMenuLog());
            return false;
        }

        private void HandleCancel()
        {
            DeSelectAllHeroes();

            if (_selectingSkill is not null)
            {
                ClearTargetSelectionAndSelectingSkill();
                return;
            }

            fsm.RequestStateChange(SkillMenuStateMachine.CharacterSelection);
        }

        private void ClearTargetSelectionAndSelectingSkill()
        {
            _selectingSkill = null;
            _skillPanel.Input.MenuConfirmedEvent += OnCastSkill;
            _input.MenuConfirmedEvent -= CastSkill;
            DisableAllSkillButtons(false);
            _skillListPanel.SelectLastSelectedOrFirstSkill();
        }
    }
}