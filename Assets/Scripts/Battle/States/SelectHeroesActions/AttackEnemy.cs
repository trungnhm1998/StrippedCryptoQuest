using System;
using CryptoQuest.Battle.Components;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class AttackEnemy : StateBase
    {
        private HeroBehaviour _hero;

        public AttackEnemy(HeroBehaviour hero, SelectHeroesActions fsm) : base(fsm)
        {
            _hero = hero;
        }

        public override void OnEnter()
        {
            Fsm.EnemyPartyManager.EnemiesPresenter.Show();
            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed += CancelPressed;
        }

        public override void OnExit()
        {
            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed -= CancelPressed;
            Fsm.EnemyPartyManager.EnemiesPresenter.Hide();
        }

        private void CancelPressed(InputAction.CallbackContext obj)
        {
            Fsm.PopState();
        }
    }
}