using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.CommandDetail;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class AttackEnemy : StateBase
    {
        private readonly HeroBehaviour _hero;
        private NormalAttackCommand _normalAttackCommand;

        public AttackEnemy(HeroBehaviour hero, SelectHeroesActions fsm) : base(fsm)
        {
            _hero = hero;
        }

        public override void OnEnter()
        {
            // this means we are coming back from the command detail screen
            var normalAttackCommand = _normalAttackCommand;
            if (normalAttackCommand != null)
            {
                _normalAttackCommand = null;
                Fsm.PopCommand();
            }

            Fsm.EnemyPartyManager.EnemiesPresenter.Show();
            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed += CancelPressed;
            UIEnemy.EnemySelected += CreateAttackCommand;
        }

        public override void OnExit()
        {
            UIEnemy.EnemySelected -= CreateAttackCommand;
            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed -= CancelPressed;
            Fsm.EnemyPartyManager.EnemiesPresenter.Hide();
        }

        private void CreateAttackCommand(EnemyBehaviour enemy)
        {
            _normalAttackCommand ??= new NormalAttackCommand(_hero.gameObject, enemy.gameObject);
            Fsm.PushCommand(_hero, _normalAttackCommand);
        }

        private void CancelPressed(InputAction.CallbackContext obj)
        {
            Fsm.PopState();
        }
    }
}