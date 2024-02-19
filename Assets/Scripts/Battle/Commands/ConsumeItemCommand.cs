using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Events;
using CryptoQuest.Inventory.Actions;
using CryptoQuest.Item.Consumable;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Battle.Commands
{
    public class ConsumeItemCommand : ICommand
    {
        private readonly Components.Character _owner;
        private ConsumableInfo _selectedItem;
        private Components.Character[] _targets;

        public ConsumeItemCommand(Components.Character owner, ConsumableInfo selectedItem,
            params Components.Character[] targets)
        {
            _targets = targets;
            _selectedItem = selectedItem;
            _owner = owner;
            BattleEventBus.RaiseEvent(new SelectedItemEvent() { ItemInfo = selectedItem });
        }

        public void Execute()
        {
            bool ableToUseOnAtLeastOneHero = false;
            foreach (var target in _targets)
            {
                if (!target.IsValid()) continue;
                BattleEventBus.RaiseEvent(new ConsumeItemEvent()
                {
                    Character = _owner,
                    ItemInfo = _selectedItem,
                    Target = target,
                });
                var spec = target.AbilitySystem.GiveAbility<ConsumableAbilitySpec>(_selectedItem.Data.Ability);
                if (spec.CanActiveAbility()) ableToUseOnAtLeastOneHero = true;
                Debug.Log($"{_owner.DisplayName} using {_selectedItem.Data.name} on {target.DisplayName}");
                spec.TryActiveAbility(_selectedItem.Data);
            }

            if (ableToUseOnAtLeastOneHero)
            {
                ActionDispatcher.Dispatch(new ItemConsumed(_selectedItem.Data));
                return;
            }

            BattleEventBus.RaiseEvent(new ConsumeItemFailEvent());
        }
    }
}