using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Ability;
using CryptoQuest.Item;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;
using System.Collections;
using CryptoQuest.System;
using CryptoQuest.Gameplay.Inventory;

namespace CryptoQuest.Battle.Commands
{
    public class ConsumeItemCommand : ICommand
    {
        private readonly Components.Character _owner;
        private ConsumableInfo _selectedItem;
        private Components.Character _target;

        public ConsumeItemCommand(Components.Character owner, ConsumableInfo selectedItem,
            Components.Character target, bool isTargetEnemy = false)
        {
            if (isTargetEnemy) owner.Targeting.Target = target;
            _target = target;
            _selectedItem = selectedItem;
            _owner = owner;
        }
        
        public IEnumerator Execute()
        {
            Debug.Log($"{_owner.name} using {_selectedItem.Data.name} on {_target.name}");
            var spec = _target.AbilitySystem.GiveAbility<ConsumableAbilitySpec>(_selectedItem.Data.Ability);
            spec.SetConsumable(_selectedItem);
            if (spec.CanActiveAbility())
            {
                _selectedItem.OnConsumed(ServiceProvider.GetService<IInventoryController>());
            }
            spec.TryActiveAbility();
            yield break;
        }
    }
}