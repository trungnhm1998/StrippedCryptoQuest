using System;
using CryptoQuest.Character.Ability;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Item;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
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

            var party = ServiceProvider.GetService<IPartyController>();
            foreach (var index in heroIndices)
            {
                var hero = party.Slots[index].HeroBehaviour;
                if (hero.IsValid() == false) continue;
                var abilitySystem = hero.GetComponent<AbilitySystemBehaviour>();
                var spec = abilitySystem.GiveAbility<ConsumableAbilitySpec>(consumable.Data.Ability);
                spec.SetConsumable(consumable);

                if (spec.CanActiveAbility() && !ableToUseOnAtLeastOneHero)
                    ableToUseOnAtLeastOneHero = true;
                spec.TryActiveAbility();
            }

            if (ableToUseOnAtLeastOneHero)
            {
                consumable.OnConsumed(_inventoryController);
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