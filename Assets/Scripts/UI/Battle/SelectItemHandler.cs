using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Skills;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.UI.Battle.CommandsMenu;
using UnityEngine;

namespace CryptoQuest.UI.Battle
{
    public class SelectItemHandler : BattleActionHandler.BattleActionHandler
    {
        [SerializeField] private BattlePanelController _panelController;
        [SerializeField] private InventorySO _inventorySO;
        private List<ExpendableItemSO> _items = new();

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
            foreach (var item in _inventorySO.Items)
            {
                if (item.ItemSO.Type == EItemType.Expendables)
                {
                    ExpendableItemSO expendableItemSO = item.ItemSO as ExpendableItemSO;
                    _items.Add(expendableItemSO);
                }
            }

            foreach (var item in _items)
            {
                var buttonInfo = new ExpendableItemAbstractButtonInfo(SetExpendableItemAbility, item);
                _buttonInfo.Add(buttonInfo);
            }

            _panelController.OpenCommandDetailPanel(_buttonInfo);
        }

        private void SetExpendableItemAbility(ExpendableItemSO itemSo)
        {
            var ability = _currentUnit.Owner.GiveAbility(itemSo.Ability);
            _currentUnit.SelectSkill(ability);
            _panelController.CloseCommandDetailPanel();
            NextHandler?.Handle(_currentUnit);
        }
    }
}