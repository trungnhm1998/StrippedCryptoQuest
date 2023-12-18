using CryptoQuest.Item.Consumable;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public class ConsumableActionBase : ActionBase
    {
        public ConsumableSO Item { get; }

        protected ConsumableActionBase(ConsumableSO item)
        {
            Item = item;
        }
    }

    public class ConsumableQuantityChangedAction : ConsumableActionBase
    {
        public int Quantity { get; }

        protected ConsumableQuantityChangedAction(ConsumableSO item, int quantity = 1) : base(item)
        {
            Quantity = quantity;
        }
    }

    public class AddConsumableAction : ConsumableQuantityChangedAction
    {
        public AddConsumableAction(ConsumableSO item, int quantity = 1) : base(item, quantity) { }
    }

    public class RemoveConsumableAction : ConsumableQuantityChangedAction
    {
        public RemoveConsumableAction(ConsumableSO item, int quantity = 1) : base(item, quantity) { }
    }

    public class ConsumableManager : MonoBehaviour
    {
        [SerializeField] private ConsumableInventory _inventory;
        private TinyMessageSubscriptionToken _addConsumable;
        private TinyMessageSubscriptionToken _removeConsumable;

        private void OnEnable()
        {
            _addConsumable = ActionDispatcher.Bind<AddConsumableAction>(AddConsumable);
            _removeConsumable = ActionDispatcher.Bind<RemoveConsumableAction>(RemoveConsumable);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_addConsumable);
            ActionDispatcher.Unbind(_removeConsumable);
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
    }
}