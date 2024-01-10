using CryptoQuest.Inventory.Actions;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Sagas.Equipment;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;

namespace CryptoQuest.Menus.DimensionalBox.Sagas
{
    public class UpdateEquipmentAction : ActionBase
    {
        public EquipmentResponse[] Equipments { get; }

        public UpdateEquipmentAction(EquipmentResponse[] equipments)
        {
            Equipments = equipments;
        }
    }

    public class UpdateInventoryOnTransferSuccessSaga : SagaBase<UpdateEquipmentAction>
    {
        protected override void HandleAction(UpdateEquipmentAction ctx)
        {
            var converter = ServiceProvider.GetService<IEquipmentResponseConverter>();
            foreach (var equipmentResponse in ctx.Equipments)
            {
                var equipment = converter.Convert(equipmentResponse);
                if (equipmentResponse.inGameStatus == (int)EEquipmentStatus.InGame)
                    AddEquipment(equipment);
                else
                    RemoveEquipment(equipment);
            }
        }

        private void AddEquipment(IEquipment equipment) => ActionDispatcher.Dispatch(new AddEquipmentAction(equipment));
        private void RemoveEquipment(IEquipment equipment) => ActionDispatcher.Dispatch(new RemoveEquipmentAction(equipment));
    }
}