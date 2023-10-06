using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.Battle.UI.SelectItem;
using CryptoQuest.Battle.UI.SelectSkill;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    public class SelectSingleEnemyToUseItem : StateBase
    {
        private readonly UIItem _selectedItem;
        private readonly SelectEnemyPresenter _selectEnemyPresenter;

        public SelectSingleEnemyToUseItem(UIItem selectedItem, HeroBehaviour hero, SelectHeroesActions fsm) :
            base(hero, fsm)
        {
            _selectedItem = selectedItem;
            _selectEnemyPresenter = fsm.BattleStateMachine.gameObject.GetComponentInChildren<SelectEnemyPresenter>();
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
            var useItemCommand = new ConsumeItemCommand(Hero, _selectedItem.Item, enemy);
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(useItemCommand);
            Fsm.GoToNextState();
        }
    }
}