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
    ///
    /// This class has a Push down state machine for each member/hero in party
    /// </summary>
    public class SelectCommand : IState
    {
        private UISelectCommand _selectCommandUI;
        private int _currentHeroIndex = 0;
        private Dictionary<int, ICommand> _heroCommands = new Dictionary<int, ICommand>();
        private BattleStateMachine _battleStateMachine;
        private IPartyController _party;
        private EnemyPartyManager _enemyPartyManager;

        public void OnEnter(BattleStateMachine battleStateMachine)
        {
            _battleStateMachine = battleStateMachine;
            _selectCommandUI = battleStateMachine.CommandUI;
            _enemyPartyManager = battleStateMachine.GetComponent<EnemyPartyManager>();
            _selectCommandUI.AttackCommandPressed += ChangeToSelectEnemyState;

            battleStateMachine.BattleUI.SetActive(true);
            _party = ServiceProvider.GetService<IPartyController>();
            EnableCommandMenu();
        }

        private void EnableCommandMenu()
        {
            _enemyPartyManager.EnemiesPresenter.Hide();
            _selectCommandUI.SetActiveCommandsMenu(true);
            _selectCommandUI.SelectFirstButton();
        }

        public void OnExit(BattleStateMachine battleStateMachine)
        {
            _selectCommandUI.AttackCommandPressed -= ChangeToSelectEnemyState;
        }

        private void ChangeToSelectEnemyState()
        {
            UIEnemy.EnemySelected += SelectEnemy;
            _battleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed += CancelPressed;
            // _stateMachine.ChangeState(new SelectEnemy());
            _selectCommandUI.SetActiveCommandsMenu(false);
            // show all enemies in details panel
            _enemyPartyManager.EnemiesPresenter.Show(_enemyPartyManager.Enemies);
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
            _battleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed -= CancelPressed;
            EnableCommandMenu();
        }
    }
}