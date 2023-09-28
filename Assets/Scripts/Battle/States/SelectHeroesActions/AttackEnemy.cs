using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.CommandDetail;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class AttackEnemy : StateBase
    {
        private readonly HeroBehaviour _hero;

        public AttackEnemy(HeroBehaviour hero, SelectHeroesActions fsm) : base(fsm)
        {
            _hero = hero;
        }

        public override void OnEnter()
        {
            Fsm.EnemyPartyManager.EnemiesPresenter.Show();
            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed += CancelPressed;
            UIEnemy.EnemySelected += CreateAttackCommand;
        }

        public override void OnExit()
        {
            OnDestroy();
        }

        public override void OnDestroy()
        {
            UIEnemy.EnemySelected -= CreateAttackCommand;
            Fsm.BattleStateMachine.BattleInput.InputActions.BattleMenu.Cancel.performed -= CancelPressed;
            Fsm.EnemyPartyManager.EnemiesPresenter.Hide();
        }

        private void CreateAttackCommand(EnemyBehaviour enemy)
        {
            var normalAttackCommand = new NormalAttackCommand(_hero.gameObject, enemy.gameObject);
            _hero.GetComponent<Components.Character>().SetCommand(normalAttackCommand);
            if (Fsm.GetNextAliveHero(out var hero))
                Fsm.PushState(new SelectCommand(hero, Fsm));
            else
            {
                Debug.Log("Battle::State::AttackEnemy cannot get next alive hero" +
                          "\nThis could be because we at the end of the party list or all heroes are dead");
                Fsm.GoToPresentState();
            }
        }

        private void CancelPressed(InputAction.CallbackContext obj)
        {
            Fsm.PopState();
        }
    }
}