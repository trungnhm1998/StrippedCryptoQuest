using CryptoQuest.Item.Consumable;
using IndiGames.Core.Events;

namespace CryptoQuest.ShopSystem.Sagas
{
    public class SellConsumableAction : ActionBase
    {
        private readonly ConsumableInfo _itemInfo;
        private readonly float _itemPrice;

        public SellConsumableAction(ConsumableInfo itemInfo, float itemPrice)
        {
            _itemInfo = itemInfo;
            _itemPrice = itemPrice;
        }

        public ConsumableInfo ItemInfo => _itemInfo;

        public float ItemPrice => _itemPrice;
    }

    public class SellConsumableSaga : SagaBase<SellConsumableAction>
    {
        protected override void HandleAction(SellConsumableAction ctx) { }
    }
}