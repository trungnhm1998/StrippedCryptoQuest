using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.Item;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class SelectSingleEnemyToUseItem : StateBase
    {
        private readonly ConsumableInfo _selectedItem;
        private readonly SelectEnemyPresenter _selectEnemyPresenter;

        public SelectSingleEnemyToUseItem(ConsumableInfo selectedItem, HeroBehaviour hero, SelectHeroesActions fsm) :
            base(hero, fsm)
        {
            _selectedItem = selectedItem;
            fsm.TryGetPresenterComponent<SelectEnemyPresenter>(out _selectEnemyPresenter);
        }

        public override void OnEnter()
        {
            _selectEnemyPresenter.Show();
            _selectEnemyPresenter.RegisterEnemySelectedCallback(CreateCommandToUseItemOnEnemy);
        }

        public override void OnExit()
        {
            _selectEnemyPresenter.Hide();
        }

        public override void OnCancel() { }

        private void CreateCommandToUseItemOnEnemy(EnemyBehaviour enemy)
        {
            var useItemCommand = new ConsumeItemCommand(Hero, _selectedItem, enemy);
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(useItemCommand);
            Fsm.GoToNextState();
        }
    }
}