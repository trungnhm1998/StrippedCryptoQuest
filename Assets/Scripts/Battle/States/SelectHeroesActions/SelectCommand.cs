using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.UI;
using CryptoQuest.Battle.UI.SelectCommand;
using CryptoQuest.Gameplay.Battle.Core.Helper;
using DG.Tweening;
using UnityEngine;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class SelectCommand : StateBase, ISelectCommandCallback
    {
        private const float SELECT_DELAY = 0.05f;
        private EnemyGroupPresenter _enemyGroupPresenter;

        public SelectCommand(HeroBehaviour hero, SelectHeroesActions fsm) : base(hero, fsm)
        {
            Fsm.TryGetComponent(out _enemyGroupPresenter);
            _heroEventObject = new HighlightHeroEvent() { Hero = hero };
        }

        private HighlightHeroEvent _heroEventObject;

        public override void OnEnter()
        {
            BattleEventBus.RaiseEvent<HighlightHeroEvent>(_heroEventObject);
            
            Fsm.SelectCommandUI.SetCharacterName(Hero.Spec.Origin.DetailInformation.LocalizedName);
            Fsm.SelectCommandUI.RegisterCallback(this);
            
            DOVirtual.DelayedCall(SELECT_DELAY, SelectButton);

            _enemyGroupPresenter.Show();
            EnableCommandMenu();
        }

        public override void OnExit()
        {
            _enemyGroupPresenter.Hide();
            Fsm.SelectCommandUI.RegisterCallback(null);
            DisableCommandMenu();
        }

        public override void OnCancel()
        {
            Fsm.PopToLastSelectCommandState();
        }

        public void OnAttackPressed()
        {
            Debug.Log("SelectCommandState::OnAttackPressed");
            DisableCommandMenu();
            Fsm.PushState(new AttackEnemy(Hero, Fsm));
        }

        public void OnSkillPressed()
        {
            Debug.Log("SelectCommandState::OnSkillPressed");
            DisableCommandMenu();
            Fsm.PushState(new SelectingSkill(Hero, Fsm));
        }

        public void OnItemPressed()
        {
            Debug.Log("SelectCommandState::OnItemPressed");
            Fsm.PushState(new SelectingItem(Hero, Fsm));
        }

        public void OnGuardPressed()
        {
            Debug.Log("SelectCommandState::OnGuardPressed");
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(new GuardCommand(Hero));
            Fsm.GoToNextState();
        }

        public void OnRetreatPressed()
        {
            Debug.Log("SelectCommandState::OnRetreatPressed");
            var highestAgi = Fsm.EnemyPartyManager.Enemies
                .GetHighestAttributeValue<EnemyBehaviour>(AttributeSets.Agility);
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(new RetreatCommand(Hero, highestAgi));

            Fsm.GoToNextState();
        }

        private void EnableCommandMenu()
        {
            Fsm.SelectCommandUI.SetActiveCommandsMenu(true);
        }

        private void DisableCommandMenu()
        {
            Fsm.SelectCommandUI.SetActiveCommandsMenu(false);
        }

        private void SelectButton()
        {
            Fsm.SelectCommandUI.SelectFirstButton();
        }
    }
}