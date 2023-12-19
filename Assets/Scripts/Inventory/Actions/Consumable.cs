using CryptoQuest.Battle.Components;
using CryptoQuest.Item.Consumable;
using IndiGames.Core.Events;

namespace CryptoQuest.Inventory.Actions
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

    public class ConsumeItemOnCharacter : ConsumableActionBase
    {
        public HeroBehaviour Hero { get; }

        public ConsumeItemOnCharacter(ConsumableSO item, HeroBehaviour hero) : base(item)
        {
            Hero = hero;
        }
    }

    public class ConsumeItemOnParty : ConsumableActionBase
    {
        public ConsumeItemOnParty(ConsumableSO item) : base(item) { }
    }

    public class ItemConsumed : ConsumableActionBase
    {
        public ItemConsumed(ConsumableSO item) : base(item) { }
    }
}