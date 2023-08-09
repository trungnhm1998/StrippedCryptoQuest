using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public class SelectItemHandler : BattleActionHandler.BattleActionHandler
    {
        [SerializeField] private BattlePanelController _panelController;
        [SerializeField] private InventorySO _inventorySO;
        private List<UsableSO> _items = new();

        private readonly List<AbstractButtonInfo> _buttonInfo = new();
        private IBattleUnit _currentUnit;

        public override void Handle(IBattleUnit currentUnit)
        {
            _currentUnit = currentUnit;
            SetupTargetButton(_currentUnit);
        }

        private void SetupTargetButton(IBattleUnit battleUnit)
        {
            _buttonInfo.Clear();
            foreach (var item in _inventorySO.UsableItems)
            {
                _items.Add(item.Item);
            }

            foreach (var item in _items)
            {
                var buttonInfo = new ExpendableItemAbstractButtonInfo(SetExpendableItemAbility, item);
                _buttonInfo.Add(buttonInfo);
            }

            _panelController.OpenCommandDetailPanel(_buttonInfo);
        }

        private void SetExpendableItemAbility(UsableSO itemSo)
        {
            var ability = _currentUnit.Owner.GiveAbility(itemSo.Ability);
            _currentUnit.SelectAbility(ability);
            _panelController.CloseCommandDetailPanel();
            NextHandler?.Handle(_currentUnit);
        }
    }
}