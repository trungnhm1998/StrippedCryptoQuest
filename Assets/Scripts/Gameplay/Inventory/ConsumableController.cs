using System;
using CryptoQuest.Character.Ability;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public class ConsumableController : MonoBehaviour
    {
        public static Action<int, ConsumableInfo> HeroConsumingItem;

        [SerializeField] private ServiceProvider _serviceProvider;

        [Header("Listening to")]
        [SerializeField] private ConsumableEventChannel _consumingItemEvent;

        [Header("Raise on")]
        [SerializeField] private ConsumableEventChannel _itemConsumedEvent;

        private IInventoryController _inventoryController;

        private void Awake()
        {
            _inventoryController = GetComponent<IInventoryController>();
            HeroConsumingItem += ConsumeItemOnHero;
        }

        private void OnDestroy()
        {
            HeroConsumingItem -= ConsumeItemOnHero;
        }

        private void ConsumeItemOnHero(int heroIndex, ConsumableInfo consumable)
        {
            Debug.Log($"Consuming {consumable.Data} on hero {heroIndex}");
            // var hero = _serviceProvider.PartyController.Party.Members[heroIndex];
            // var spec = (ConsumableAbilitySpec)consumable.Data.Ability
            //     .GetAbilitySpec(null); // Consumable could not have owner
            // spec.Consume(hero.CharacterComponent.GameplayAbilitySystem);
        }

        private void OnEnable()
        {
            _consumingItemEvent.EventRaised += ConsumeItem;
        }

        private void OnDisable()
        {
            _consumingItemEvent.EventRaised -= ConsumeItem;
        }

        private void ConsumeItem(ConsumableInfo consumable)
        {
            _inventoryController.Remove(consumable);
            _itemConsumedEvent.RaiseEvent(consumable);
        }
    }
}