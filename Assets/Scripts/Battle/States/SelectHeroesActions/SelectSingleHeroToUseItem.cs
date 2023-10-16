using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.SelectHero;
using CryptoQuest.Battle.UI.SelectItem;
using CryptoQuest.Battle.UI.SelectSkill;
using CryptoQuest.Item;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    internal class SelectSingleHeroToUseItem : StateBase
    {
        private readonly ConsumableInfo _selectedItem;
        private readonly SelectHeroPresenter _selectHeroPresenter;
        private readonly SelectItemPresenter _itemPresenter;

        public SelectSingleHeroToUseItem(SelectItemPresenter itemPresenter, HeroBehaviour hero, SelectHeroesActions fsm) :
            base(hero, fsm)
        {
            _itemPresenter = itemPresenter;
            _selectedItem = itemPresenter.SelectedItem;
            _selectHeroPresenter = Fsm.BattleStateMachine.SelectHeroPresenter;
        }

        public override void OnEnter()
        {
            _selectHeroPresenter.Show(_selectedItem.Data.DisplayName);
            _itemPresenter.SetActiveScroll(true);
            _itemPresenter.SetInteractive(false);
            SelectHeroPresenter.ConfirmSelectCharacter += UseItemOnHero;
        }

        public override void OnExit()
        {
            _selectHeroPresenter.Hide();
            _itemPresenter.SetActiveScroll(false);
            SelectHeroPresenter.ConfirmSelectCharacter -= UseItemOnHero;
        }

        public override void OnCancel() { }

        private void UseItemOnHero(HeroBehaviour selectedHero)
        {
            var useItemCommand = new ConsumeItemCommand(Hero, _selectedItem, selectedHero);
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(useItemCommand);
            Fsm.GoToNextState();
        }
    }
}