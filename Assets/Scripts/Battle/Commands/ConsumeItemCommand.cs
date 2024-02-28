﻿using CryptoQuest.Battle.Components;
using CryptoQuest.Item;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;
using System.Collections;
using CryptoQuest.AbilitySystem.Abilities;
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
            BattleEventBus.RaiseEvent<SelectedItemEvent>(new SelectedItemEvent() { ItemInfo = selectedItem });
        }

        public void Execute()
        {
            var inventoryController = ServiceProvider.GetService<IInventoryController>();
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
                return;
            }

            BattleEventBus.RaiseEvent(new ConsumeItemFailEvent());
        }
    }
}