using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Ability;
using CryptoQuest.Item;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;
using System.Collections;
using CryptoQuest.System;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Battle.Events;

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
        }
        
        public IEnumerator Execute()
        {
            var inventoryController = ServiceProvider.GetService<IInventoryController>();
            bool ableToUseOnAtLeastOneHero = false;
            foreach (var target in _targets)
            {
                BattleEventBus.RaiseEvent(new ConsumeItemEvent()
                {
                    Character = _owner,
                    ItemInfo = _selectedItem,
                    Target = target,
                });
                Debug.Log($"{_owner.DisplayName} using {_selectedItem.Data.name} on {target.DisplayName}");
                var spec = target.AbilitySystem.GiveAbility<ConsumableAbilitySpec>(_selectedItem.Data.Ability);
                spec.SetConsumable(_selectedItem);
                if (spec.CanActiveAbility() && !ableToUseOnAtLeastOneHero)
                {
                    ableToUseOnAtLeastOneHero = true;
                }
                spec.TryActiveAbility();
            }
            
            if (ableToUseOnAtLeastOneHero)
            {
                _selectedItem.OnConsumed(inventoryController);
            }
           
            yield break;
        }
    }
}