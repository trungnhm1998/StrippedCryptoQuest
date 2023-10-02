using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.CommandDetail;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class AttackEnemy : StateBase
    {
        private readonly SelectEnemyPresenter _selectEnemyPresenter;

        public AttackEnemy(HeroBehaviour hero, SelectHeroesActions fsm) : base(hero, fsm)
        {
            _selectEnemyPresenter = fsm.BattleStateMachine.gameObject.GetComponentInChildren<SelectEnemyPresenter>();
        }

        public override void OnEnter()
        {
            _selectEnemyPresenter.Show();
            _selectEnemyPresenter.RegisterEnemySelectedCallback(CreateAttackCommand);
        }

        public override void OnExit()
        {
            _selectEnemyPresenter.Hide();
        }

        private void CreateAttackCommand(EnemyBehaviour enemy)
        {
            var normalAttackCommand = new NormalAttackCommand(Hero, enemy);
            Hero.TryGetComponent<Components.Character>(out var character);
            character.SetCommand(normalAttackCommand);
            Fsm.GoToNextState();
        }

        public override void OnCancel() { }
    }
}