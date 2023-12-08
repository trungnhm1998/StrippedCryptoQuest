using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Sagas.Profile;
using IndiGames.Core.Common;
using IndiGames.Core.Events;

namespace CryptoQuest.BlackSmith.Evolve.Sagas
{
    public class AddEquipmentSaga : SagaBase<AddEquipment>
    {
        private IInventoryController _inventoryController;
        private IEquipmentResponseConverter _responseConverter;
        
        protected override void HandleAction(AddEquipment ctx)
        {
            if (ctx.EquipmentData == null) return;
            
            _inventoryController ??= ServiceProvider.GetService<IInventoryController>();
            _responseConverter ??= ServiceProvider.GetService<IEquipmentResponseConverter>();

            var equipment = _responseConverter.Convert(ctx.EquipmentData);
            equipment.AddToInventory(_inventoryController);
        }
    }
}
