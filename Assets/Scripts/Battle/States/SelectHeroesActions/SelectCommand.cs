using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using CryptoQuest.Battle.Extensions;
using CryptoQuest.Battle.UI;
using CryptoQuest.Battle.UI.SelectCommand;
using DG.Tweening;
using UnityEngine;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class SelectCommand : StateBase, ISelectCommandCallback
    {
        private const float SHOW_DELAY = 0.05f;
        private EnemyGroupPresenter _enemyGroupPresenter;
        private SelectCommandPresenter _selectCommandPresenter;
        private UISelectCommand _selectCommandUI;

        public SelectCommand(HeroBehaviour hero, SelectHeroesActions fsm) : base(hero, fsm)
        {
            Fsm.TryGetPresenterComponent(out _enemyGroupPresenter);
            Fsm.TryGetPresenterComponent(out _selectCommandPresenter);
            _heroEventObject = new HighlightHeroEvent() { Hero = hero };
        }

        private HighlightHeroEvent _heroEventObject;

        public override void OnEnter()
        {
            _selectCommandUI = Fsm.SelectCommandUI;

            BattleEventBus.RaiseEvent<HighlightHeroEvent>(_heroEventObject);
            
            _selectCommandUI.SetCharacterName(Hero.Spec.Origin.DetailInformation.LocalizedName);
            _selectCommandUI.RegisterCallback(this);
            
            _selectCommandUI.SelectFirstButton();

            DOVirtual.DelayedCall(SHOW_DELAY, ShowUI);
        }

        public override void OnExit()
        {
            _enemyGroupPresenter.Hide();
            _selectCommandUI.RegisterCallback(null);
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
            _selectCommandUI.SetActiveCommandsMenu(true);
        }

        private void DisableCommandMenu()
        {
            _selectCommandUI.SetActiveCommandsMenu(false);
        }

        private void ShowUI()
        {
            _enemyGroupPresenter.Show();
            EnableCommandMenu();
            _selectCommandPresenter.CheckActiveButtons(Hero);
        }
    }
}