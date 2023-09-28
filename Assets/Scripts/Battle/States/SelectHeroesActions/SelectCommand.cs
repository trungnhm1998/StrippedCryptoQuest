using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.SelectCommand;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class SelectCommand : StateBase, ISelectCommandCallback
    {
        private readonly HeroBehaviour _hero;
        private HeroBehaviour _currentHero;

        public SelectCommand(HeroBehaviour hero, SelectHeroesActions fsm) : base(fsm)
        {
            _hero = hero;
        }

        public override void OnEnter()
        {
            Fsm.SelectCommandUI.RegisterCallback(this);
            EnableCommandMenu();
        }

        public override void OnExit()
        {
            Fsm.SelectCommandUI.RegisterCallback(null);
            DisableCommandMenu();
        }

        private void ChangeToSelectEnemyState()
        {
            // UIEnemy.EnemySelected += SelectEnemy;
            // _battleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed += CancelPressed;
            // _stateMachine.ChangeState(new SelectEnemy());
            // _selectCommandUI.SetActiveCommandsMenu(false);
            // show all enemies in details panel
            // _enemyPartyManager.EnemiesPresenter.Show(_enemyPartyManager.Enemies);
        }

        private void SelectEnemy(EnemyBehaviour enemy)
        {
            // UIEnemy.EnemySelected -= SelectEnemy;
            // ServiceProvider.GetService<IPartyController>().TryGetMemberAtIndex(_currentHeroIndex, out var hero);
            // _heroCommands[_currentHeroIndex] = new NormalAttackCommand(hero.GameObject, enemy.gameObject);
            // _currentHeroIndex++;
        }

        private void CancelPressed(InputAction.CallbackContext obj)
        {
            // UIEnemy.EnemySelected -= SelectEnemy;
            // _battleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed -= CancelPressed;
            // EnableCommandMenu();
        }

        public void OnAttackPressed()
        {
            Debug.Log("SelectCommandState::OnAttackPressed");
            Fsm.PushState(new AttackEnemy(_hero, Fsm));
        }

        public void OnSkillPressed()
        {
            Debug.Log("SelectCommandState::OnSkillPressed");
            Fsm.PushState(new CastSkillOnEnemy(_hero, Fsm));
        }

        public void OnItemPressed()
        {
            Debug.Log("SelectCommandState::OnItemPressed");
        }

        public void OnGuardPressed()
        {
            Debug.Log("SelectCommandState::OnGuardPressed");
            Fsm.AddCommand(_hero, new GuardCommand(_hero));
            NextHero();
        }

        public void OnRetreatPressed()
        {
            Debug.Log("SelectCommandState::OnRetreatPressed");
            NextHero();
        }

        private void NextHero()
        {
            if (Fsm.GetNextAliveHero(out var nextHero))
                Fsm.PushState(new SelectCommand(nextHero, Fsm));
            else
                Fsm.GoToPresentState();
        }


        private void EnableCommandMenu()
        {
            // _enemyPartyManager.EnemiesPresenter.Hide();
            Fsm.SelectCommandUI.SetActiveCommandsMenu(true);
            Fsm.SelectCommandUI.SelectFirstButton();
        }


        private void DisableCommandMenu()
        {
            Fsm.SelectCommandUI.SetActiveCommandsMenu(true);
        }
    }
}