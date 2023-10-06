using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.SelectHero;
using CryptoQuest.Battle.UI.SelectItem;
using CryptoQuest.Battle.UI.SelectSkill;
using UnityEngine.InputSystem;

namespace CryptoQuest.Battle.States.SelectHeroesActions
{
    internal class SelectSingleHeroToUseItem : StateBase
    {
        private readonly UIItem _selectedItemUI;
        private readonly SelectHeroPresenter _selectHeroPresenter;
        private readonly SelectItemPresenter _itemPresenter;

        public SelectSingleHeroToUseItem(SelectItemPresenter itemPresenter, HeroBehaviour hero, SelectHeroesActions fsm) :
            base(hero, fsm)
        {
            _itemPresenter = itemPresenter;
            _selectedItemUI = itemPresenter.LastSelectedItem;
            _selectHeroPresenter = Fsm.BattleStateMachine.SelectHeroPresenter;
        }

        public override void OnEnter()
        {
            _selectHeroPresenter.Show(_selectedItemUI.Item.Data.DisplayName);
            _itemPresenter.SetActiveScroll(true);
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
            var useItemCommand = new ConsumeItemCommand(Hero, _selectedItemUI.Item, selectedHero);
            Hero.TryGetComponent(out CommandExecutor commandExecutor);
            commandExecutor.SetCommand(useItemCommand);
            Fsm.GoToNextState();
        }
    }
}