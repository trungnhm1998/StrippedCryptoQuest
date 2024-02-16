using CryptoQuest.Inventory.Actions;
using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Sagas.Equipment;
using CryptoQuest.Sagas.MagicStone;
using CryptoQuest.Sagas.Objects;
using IndiGames.Core.Common;
using IndiGames.Core.Events;
using UnityEngine;

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
        [SerializeField] private EquipmentInventory _equipmentInventory;

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

            // Because when transfer equipment with stone, the stone is not attach any more
            // So we need to fetch again to update the list of stone
            ActionDispatcher.Dispatch(new FetchProfileMagicStonesAction());
        }

        private void AddEquipment(IEquipment equipment)
        {
            // Only add if not already in inventory
            var matchEquipment = _equipmentInventory.Equipments.Find(e => e.Id == equipment.Id);
            if (matchEquipment != null)
                return;

            ActionDispatcher.Dispatch(new AddEquipmentAction(equipment));
        }

        private void RemoveEquipment(IEquipment equipment) => ActionDispatcher.Dispatch(new RemoveEquipmentAction(equipment));
    }
}