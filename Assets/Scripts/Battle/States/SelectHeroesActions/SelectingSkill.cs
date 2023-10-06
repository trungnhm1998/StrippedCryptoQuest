﻿using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.SelectSkill;
using UnityEngine;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class SelectingSkill : StateBase
    {
        private readonly SelectSkillPresenter _skillPresenter;

        public SelectingSkill(HeroBehaviour hero, SelectHeroesActions fsm) : base(hero, fsm)
        {
            Fsm.TryGetComponent(out _skillPresenter);
        }

        public override void OnEnter()
        {
            _skillPresenter.Show(Hero);
            _skillPresenter.SelectSingleEnemyCallback = SelectEnemyToCastSkillOn;
            _skillPresenter.SelectSingleHeroCallback = SelectHeroToCastSkillOn;
            _skillPresenter.SelectAllHeroCallback = SelectAllHeroToCastSkillOn;
            _skillPresenter.SelectAllEnemyCallback = SelectAllEnemyToCastSkillOn;
            _skillPresenter.SelectEnemyGroupCallback = SelectEnemyGroupToCastSkillOn;
        }

        public override void OnExit()
        {
            _skillPresenter.Hide();
        }
        
        public override void OnCancel() { }

        private void SelectEnemyToCastSkillOn(UISkill skillUI)
        {
            _skillPresenter.Show(Hero, false);
            Debug.Log("SelectingSkill::SelectEnemyToCastSkillOn");
            Fsm.PushState(new SelectSingleEnemyToCastSkill(skillUI, Hero, Fsm));
        }

        private void SelectHeroToCastSkillOn(UISkill skillUI)
        {
            _skillPresenter.Show(Hero, false);
            Debug.Log("SelectingSkill::SelectHeroToCastSkillOn");
            Fsm.PushState(new SelectSingleHeroToCastSkill(skillUI, Hero, Fsm));
        }

        private void SelectAllHeroToCastSkillOn(UISkill skillUI)
        {
            _skillPresenter.Show(Hero, false);
            Debug.Log("SelectingSkill::SelectAllHeroToCastSkillOn");

            var heroes = Fsm.PlayerParty.OrderedAliveMembers.ToArray();
            CreateMultipleTargetCommand(skillUI, heroes);
        }

        private void SelectAllEnemyToCastSkillOn(UISkill skillUI)
        {
            _skillPresenter.Show(Hero, false);
            Debug.Log("SelectingSkill::SelectAllEnemyToCastSkillOn");
        
            var enemies = Fsm.EnemyPartyManager.Enemies.ToArray();
            CreateMultipleTargetCommand(skillUI, enemies);
        }

        private void SelectEnemyGroupToCastSkillOn(UISkill skillUI)
        {
            _skillPresenter.Show(Hero, false);
            Debug.Log("SelectingSkill::SelectEnemyGroupToCastSkillOn");
            Fsm.PushState(new SelectEnemyGroupToCastSkill(skillUI.Skill, Hero, Fsm));
        }

        private void CreateMultipleTargetCommand(UISkill skillUI, params Components.Character[] characters)
        {
            var useItemCommand = new MultipleTargetCastSkillCommand(Hero, skillUI.Skill, characters);
            Hero.SetCommand(useItemCommand);
            Fsm.GoToNextState();
        }

    }
}