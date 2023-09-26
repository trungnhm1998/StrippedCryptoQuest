using System.Collections.Generic;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.Battle.UI.SelectCommand;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.System;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States
{
    /// <summary>
    /// Select the command of all the heroes in party
    ///
    /// only exit this state after selected commands for all heroes
    /// </summary>
    public class SelectCommand : IState
    {
        private UISelectCommand _selectCommandUI;
        private int _currentHeroIndex = 0;
        private Dictionary<int, ICommand> _heroCommands = new Dictionary<int, ICommand>();
        private StateMachine _stateMachine;
        private IPartyController _party;

        public SelectCommand(UISelectCommand fsmCommandUI)
        {
            _selectCommandUI = fsmCommandUI;
        }

        public void OnEnter(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _selectCommandUI.AttackCommandPressed += ChangeToSelectEnemyState;

            stateMachine.BattleUI.SetActive(true);
            _party = ServiceProvider.GetService<IPartyController>();
            EnableCommandMenu();
        }

        private void EnableCommandMenu()
        {
            _stateMachine.EnemiesPresenter.Hide();
            _selectCommandUI.SetActiveCommandsMenu(true);
            _selectCommandUI.SelectFirstButton();
        }

        public void OnExit(StateMachine stateMachine)
        {
            _selectCommandUI.AttackCommandPressed -= ChangeToSelectEnemyState;
        }

        private void ChangeToSelectEnemyState()
        {
            UIEnemy.EnemySelected += SelectEnemy;
            _stateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed += CancelPressed;
            // _stateMachine.ChangeState(new SelectEnemy());
            _selectCommandUI.SetActiveCommandsMenu(false);
            // show all enemies in details panel
            _stateMachine.EnemiesPresenter.Show();
        }

        private void SelectEnemy(EnemyBehaviour enemy)
        {
            UIEnemy.EnemySelected -= SelectEnemy;
            // ServiceProvider.GetService<IPartyController>().TryGetMemberAtIndex(_currentHeroIndex, out var hero);
            // _heroCommands[_currentHeroIndex] = new NormalAttackCommand(hero.GameObject, enemy.gameObject);
            // _currentHeroIndex++;
        }

        private void CancelPressed(InputAction.CallbackContext obj)
        {
            UIEnemy.EnemySelected -= SelectEnemy;
            _stateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed -= CancelPressed;
            EnableCommandMenu();
        }
    }
}