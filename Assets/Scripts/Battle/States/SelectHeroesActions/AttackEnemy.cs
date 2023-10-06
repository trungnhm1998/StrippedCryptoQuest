using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.CommandDetail;

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
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(normalAttackCommand);
            Fsm.GoToNextState();
        }

        public override void OnCancel() { }
    }
}