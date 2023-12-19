using CryptoQuest.Inventory.Actions;
using CryptoQuest.Item.Consumable;
using IndiGames.Core.Events;

namespace CryptoQuest.ShopSystem.Sagas
{
    public class SellConsumableAction : ActionBase
    {
        public ConsumableSO Item { get; }
        public int Quantity { get; }

        public SellConsumableAction(ConsumableSO item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }

    public class SellConsumableSaga : SagaBase<SellConsumableAction>
    {
        /// <summary>
        /// Add gold and remove item from inventory
        /// </summary>
        /// <param name="ctx"></param>
        protected override void HandleAction(SellConsumableAction ctx)
        {
            var item = ctx.Item;
            var quantity = ctx.Quantity;

            ActionDispatcher.Dispatch(new AddGoldAction(item.Price * quantity));
            ActionDispatcher.Dispatch(new RemoveConsumableAction(item, quantity));
        }
    }
}