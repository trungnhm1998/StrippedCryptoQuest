using System;
using CryptoQuest.Character.Ability;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public class ConsumableController : MonoBehaviour
    {
        public static event Action<ConsumableInfo, int[]> HeroConsumingItem;
        public static event Action<ConsumableInfo> ConsumingItem;
        public static event Action<ConsumableInfo> ConsumedItem;

        public static void OnConsumeItem(ConsumableInfo consumable) => ConsumingItem?.Invoke(consumable);

        public static void OnConsumeItem(ConsumableInfo consumable, params int[] heroIndices)
            => HeroConsumingItem?.Invoke(consumable, heroIndices);

        [SerializeField] private ServiceProvider _serviceProvider;

        [Header("Raise on")]
        [SerializeField] private ConsumableEventChannel _itemConsumedEvent;

        private IInventoryController _inventoryController;

        private void Awake()
        {
            _inventoryController = GetComponent<IInventoryController>();
            HeroConsumingItem += ConsumeItem;
            ConsumingItem += ConsumeItem;
        }

        private void OnDestroy()
        {
            HeroConsumingItem -= ConsumeItem;
            ConsumingItem -= ConsumeItem;
        }

        private void ConsumeItem(ConsumableInfo consumable, int[] heroIndices)
        {
            bool ableToUseOnAtLeastOneHero = false;

            foreach (var index in heroIndices)
            {
                var hero = _serviceProvider.PartyController.Party.Members[index];
                if (hero.IsValid() == false) continue;
                var spec = hero
                    .CharacterComponent
                    .GameplayAbilitySystem
                    .GiveAbility<ConsumableAbilitySpec>(consumable.Data.Ability);
                spec.SetConsumable(consumable);
                spec.TryActiveAbility();

                if (spec.CanActiveAbility() && !ableToUseOnAtLeastOneHero)
                    ableToUseOnAtLeastOneHero = true;
            }

            if (ableToUseOnAtLeastOneHero && _inventoryController.Remove(consumable))
            {
                Debug.Log($"Consuming {consumable.Data} on {heroIndices.Length} heroes");
            }

            // TODO: Raise consumed failed?
            _itemConsumedEvent.RaiseEvent(consumable);
        }

        private void ConsumeItem(ConsumableInfo consumable)
        {
            if (_inventoryController.Remove(consumable))
                _itemConsumedEvent.RaiseEvent(consumable);
        }
    }
}