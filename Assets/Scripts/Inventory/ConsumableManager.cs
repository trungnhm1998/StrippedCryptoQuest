using CryptoQuest.Inventory.Actions;
using CryptoQuest.Inventory.ScriptableObjects.Item.Type;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Inventory
{
    public class ConsumableManager : MonoBehaviour
    {
        [SerializeField] private ConsumableInventory _inventory;
        private TinyMessageSubscriptionToken _addConsumable;
        private TinyMessageSubscriptionToken _removeConsumable;
        private TinyMessageSubscriptionToken _itemConsumed;

        private void OnEnable()
        {
            _addConsumable = ActionDispatcher.Bind<AddConsumableAction>(AddConsumable);
            _removeConsumable = ActionDispatcher.Bind<RemoveConsumableAction>(RemoveConsumable);
            _itemConsumed = ActionDispatcher.Bind<ItemConsumed>(RemoveWhenConsumed);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_addConsumable);
            ActionDispatcher.Unbind(_removeConsumable);
            ActionDispatcher.Unbind(_itemConsumed);
        }

        private void AddConsumable(AddConsumableAction ctx)
        {
            var item = ctx.Item;
            var quantity = ctx.Quantity;

            _inventory.Add(item, quantity);
        }

        private void RemoveConsumable(RemoveConsumableAction ctx)
        {
            var item = ctx.Item;
            var quantity = ctx.Quantity;

            _inventory.Remove(item, quantity);
        }
        
        private void RemoveWhenConsumed(ItemConsumed ctx)
        {
            if (ctx.Item.Type == EConsumableType.Consumable)
                _inventory.Remove(ctx.Item);
        }

    }
}