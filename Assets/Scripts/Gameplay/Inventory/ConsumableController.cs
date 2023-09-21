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

        [Header("Listening to")]
        [SerializeField] private ConsumableEventChannel _consumingItemEvent;

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
            // TODO: Make sure they the item consumed before remove
            if (!_inventoryController.Remove(consumable)) return;
            // TODO: Make sure all heroes are alive
            Debug.Log($"Consuming {consumable.Data} on {heroIndices.Length} heroes");
            foreach (var index in heroIndices)
            {
                var hero = _serviceProvider.PartyController.Party.Members[index];
                if (hero.IsValid() == false) continue;
                var spec = consumable.Data.Ability.GetAbilitySpec(consumable, hero.CharacterComponent.GameplayAbilitySystem);
                spec.TryActiveAbility();
            }
        }

        private void ConsumeItem(ConsumableInfo consumable)
        {
            if (_inventoryController.Remove(consumable))
                _itemConsumedEvent.RaiseEvent(consumable);
        }
    }
}