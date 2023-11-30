using System;
using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Battle.Components;
using CryptoQuest.Item;
using CryptoQuest.Item.Consumable;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public class ConsumableController : MonoBehaviour
    {
        public static event Action<ConsumableInfo, List<HeroBehaviour>> HeroConsumingItem;
        public static event Action<ConsumableInfo> ConsumingItem;
        public static event Action<ConsumableInfo> ConsumedItem;

        public static void OnConsumeItem(ConsumableInfo consumable) => ConsumingItem?.Invoke(consumable);

        public static void OnConsumeItem(ConsumableInfo consumable, List<HeroBehaviour> heroes)
            => HeroConsumingItem?.Invoke(consumable, heroes);

        [Header("Raise on")]
        [SerializeField] private ConsumableEventChannel _itemConsumedEvent;

        private IInventoryController _inventoryController;

        private IInventoryController InventoryController =>
            _inventoryController ??= ServiceProvider.GetService<IInventoryController>();

        private void Awake()
        {
            HeroConsumingItem += ConsumeItem;
            ConsumingItem += ConsumeItem;
        }

        private void OnDestroy()
        {
            HeroConsumingItem -= ConsumeItem;
            ConsumingItem -= ConsumeItem;
        }

        private void ConsumeItem(ConsumableInfo consumable, List<HeroBehaviour> heroes)
        {
            bool ableToUseOnAtLeastOneHero = false;
            foreach (var hero in heroes)
            {
                var abilitySystem = hero.GetComponent<AbilitySystemBehaviour>();
                var spec = abilitySystem.GiveAbility<ConsumableAbilitySpec>(consumable.Data.Ability);
                spec.SetConsumable(consumable);

                if (spec.CanActiveAbility() && !ableToUseOnAtLeastOneHero)
                    ableToUseOnAtLeastOneHero = true;
                spec.TryActiveAbility();
            }

            if (ableToUseOnAtLeastOneHero)
            {
                consumable.OnConsumed(InventoryController);
                Debug.Log($"Consuming {consumable.Data} on {heroes.Count} heroes");
            }

            // TODO: Raise consumed failed?
            _itemConsumedEvent.RaiseEvent(consumable);
        }

        private void ConsumeItem(ConsumableInfo consumable)
        {
            if (InventoryController.Remove(consumable))
                _itemConsumedEvent.RaiseEvent(consumable);
        }
    }
}