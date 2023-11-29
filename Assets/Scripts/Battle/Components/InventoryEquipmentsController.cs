using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;

namespace CryptoQuest.Battle.Components
{
    public class InventoryEquipmentsController : CharacterComponentBase
    {
        private EquipmentsController _equipmentsController;
        private IInventoryController _inventoryController;

        public override void Init()
        {
            _inventoryController ??= ServiceProvider.GetService<IInventoryController>();
            _equipmentsController = Character.GetComponent<EquipmentsController>();

            _equipmentsController.Equipped += RemoveFromInventory;
            _equipmentsController.Removed += AddToInventory;
        }

        protected override void OnReset()
        {
            _equipmentsController.Equipped += RemoveFromInventory;
            _equipmentsController.Removed += AddToInventory;
        }

        private void RemoveFromInventory(IEquipment equipment) => equipment.RemoveFromInventory(_inventoryController);

        private void AddToInventory(IEquipment equipment) => equipment.AddToInventory(_inventoryController);
    }
}