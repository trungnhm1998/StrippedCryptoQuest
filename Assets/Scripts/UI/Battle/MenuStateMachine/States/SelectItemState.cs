using CryptoQuest.UI.Battle.CommandsMenu;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.Helper;
using UnityEngine;

namespace CryptoQuest.UI.Battle.MenuStateMachine.States
{
    public class SelectItemState : SelectStateBase
    {
        private readonly InventorySO _inventorySO;

        public SelectItemState(BattleMenuStateMachine stateMachine, InventorySO inventory) : base(stateMachine)
        {
            _inventorySO = inventory;
        }

        protected override void SetupButtonsInfo()
        {
            foreach (var itemInfo in _inventorySO.GetItemsInBattle())
            {
                var buttonInfo = new ExpendableItemAbstractButtonInfo(SetExpendableItemAbility, itemInfo);
                _buttonInfos.Add(buttonInfo);
            }
        }

        private void SetExpendableItemAbility(UsableSO itemSo)
        {
            // System shouln't have item's ability because 
            // if item is used system dont have that ability any more
            var ability = itemSo.Ability.GetAbilitySpec(_currentUnit.Owner);
            _currentUnit.SelectAbility(ability);
            HandleTarget();
        }
    }
}