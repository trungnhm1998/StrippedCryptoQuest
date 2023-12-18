using CryptoQuest.Item.Equipment;
using IndiGames.Core.Events;

namespace CryptoQuest.ShopSystem.Sagas
{
    public class SellEquipmentAction : ActionBase
    {
        private readonly IEquipment _itemInfo;
        private readonly float _itemPrice;

        public SellEquipmentAction(IEquipment itemInfo, float itemPrice)
        {
            _itemInfo = itemInfo;
            _itemPrice = itemPrice;
        }

        public IEquipment ItemInfo => _itemInfo;

        public float ItemPrice => _itemPrice;
    }

    public class SellEquipmentSaga : SagaBase<SellEquipmentAction>
    {
        protected override void HandleAction(SellEquipmentAction ctx) { }
    }
}