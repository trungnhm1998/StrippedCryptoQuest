using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Commands;
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
            _skillPresenter.SelectSelfCallback = SelectSelfToCastSkillOn;
        }

        public override void OnExit()
        {
            _skillPresenter.Hide();
        }
        
        public override void OnCancel() { }

        private void SelectEnemyToCastSkillOn(CastSkillAbility skill)
        {
            Debug.Log("SelectingSkill::SelectEnemyToCastSkillOn");
            Fsm.PushState(new SelectSingleEnemyToCastSkill(skill, Hero, Fsm));
        }

        private void SelectHeroToCastSkillOn(CastSkillAbility skill)
        {
            Debug.Log("SelectingSkill::SelectHeroToCastSkillOn");
            Fsm.PushState(new SelectSingleHeroToCastSkill(skill, Hero, Fsm));
        }

        private void SelectAllHeroToCastSkillOn(CastSkillAbility skill)
        {
            Debug.Log("SelectingSkill::SelectAllHeroToCastSkillOn");

            var heroes = Fsm.PlayerParty.OrderedAliveMembers.ToArray();
            CreateMultipleTargetCommand(skill, heroes);
        }

        private void SelectAllEnemyToCastSkillOn(CastSkillAbility skill)
        {
            Debug.Log("SelectingSkill::SelectAllEnemyToCastSkillOn");
        
            var enemies = Fsm.EnemyPartyManager.Enemies.ToArray();
            CreateMultipleTargetCommand(skill, enemies);
        }

        private void SelectEnemyGroupToCastSkillOn(CastSkillAbility skill)
        {
            Debug.Log("SelectingSkill::SelectEnemyGroupToCastSkillOn");
            Fsm.PushState(new SelectEnemyGroupToCastSkill(skill, Hero, Fsm));
        }

        private void CreateMultipleTargetCommand(CastSkillAbility skill, params Components.Character[] characters)
        {
            var castSkillCommand = new MultipleTargetCastSkillCommand(Hero, skill, characters);
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(castSkillCommand);
            Fsm.GoToNextState();
        }

        private void SelectSelfToCastSkillOn(CastSkillAbility skill)
        {
            Debug.Log("SelectingSkill::SelectSelfToCastSkillOn");
            var castSkillCommand = new CastSkillCommand(Hero, skill, Hero);
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(castSkillCommand);
            Fsm.GoToNextState();
        }
    }
}