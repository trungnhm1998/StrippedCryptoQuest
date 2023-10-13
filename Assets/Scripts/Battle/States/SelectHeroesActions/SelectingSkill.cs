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

        private void SelectEnemyToCastSkillOn(UISkill skillUI)
        {
            Debug.Log("SelectingSkill::SelectEnemyToCastSkillOn");
            Fsm.PushState(new SelectSingleEnemyToCastSkill(skillUI, Hero, Fsm));
        }

        private void SelectHeroToCastSkillOn(UISkill skillUI)
        {
            Debug.Log("SelectingSkill::SelectHeroToCastSkillOn");
            Fsm.PushState(new SelectSingleHeroToCastSkill(skillUI, Hero, Fsm));
        }

        private void SelectAllHeroToCastSkillOn(UISkill skillUI)
        {
            Debug.Log("SelectingSkill::SelectAllHeroToCastSkillOn");

            var heroes = Fsm.PlayerParty.OrderedAliveMembers.ToArray();
            CreateMultipleTargetCommand(skillUI, heroes);
        }

        private void SelectAllEnemyToCastSkillOn(UISkill skillUI)
        {
            Debug.Log("SelectingSkill::SelectAllEnemyToCastSkillOn");
        
            var enemies = Fsm.EnemyPartyManager.Enemies.ToArray();
            CreateMultipleTargetCommand(skillUI, enemies);
        }

        private void SelectEnemyGroupToCastSkillOn(UISkill skillUI)
        {
            Debug.Log("SelectingSkill::SelectEnemyGroupToCastSkillOn");
            Fsm.PushState(new SelectEnemyGroupToCastSkill(skillUI.Skill, Hero, Fsm));
        }

        private void CreateMultipleTargetCommand(UISkill skillUI, params Components.Character[] characters)
        {
            var castSkillCommand = new MultipleTargetCastSkillCommand(Hero, skillUI.Skill, characters);
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(castSkillCommand);
            Fsm.GoToNextState();
        }

        private void SelectSelfToCastSkillOn(UISkill skillUI)
        {
            Debug.Log("SelectingSkill::SelectSelfToCastSkillOn");
            var castSkillCommand = new CastSkillCommand(Hero, skillUI.Skill, Hero);
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(castSkillCommand);
            Fsm.GoToNextState();
        }
    }
}