using CryptoQuest.UI.Battle.CommandsMenu;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;

namespace CryptoQuest.UI.Battle.MenuStateMachine.States
{
    public class SelectItemState : SelectStateBase
    {
        private readonly InventorySO _inventorySO;

        private readonly List<UsableSO> _items = new();

        public SelectItemState(BattleMenuStateMachine stateMachine, InventorySO inventory) : base(stateMachine)
        {
            _inventorySO = inventory;
        }

        protected override void SetupButtonsInfo()
        {
            _items.Clear();
            foreach (var item in _inventorySO.UsableItems)
            {
                _items.Add(item.Item);
            }

            foreach (var item in _items)
            {
                var buttonInfo = new ExpendableItemAbstractButtonInfo(SetExpendableItemAbility, item);
                _buttonInfos.Add(buttonInfo);
            }
        }

        private void SetExpendableItemAbility(UsableSO itemSo)
        {
            var ability = itemSo.Ability.GetAbilitySpec(_currentUnit.Owner);
            _currentUnit.SelectAbility(ability);
            _battlePanelController.CloseCommandDetailPanel();
        }
    }
}