using CryptoQuest.Item.Equipment;

namespace CryptoQuest.Battle.Components
{
    public class InventoryEquipmentsController : CharacterComponentBase
    {
        private EquipmentsController _equipmentsController;

        protected override void OnInit()
        {
            _equipmentsController = Character.GetComponent<EquipmentsController>();

            _equipmentsController.Equipped += RemoveFromInventory;
            _equipmentsController.Removed += AddToInventory;
        }

        protected override void OnReset()
        {
            _equipmentsController.Equipped -= RemoveFromInventory;
            _equipmentsController.Removed -= AddToInventory;
        }

        private void RemoveFromInventory(IEquipment equipment)
        {
            // TODO: Implement
            // _inventoryController.Remove(equipment);
        }

        private void AddToInventory(IEquipment equipment)
        {
            // TODO: Implement
            // _inventoryController.Add(equipment);
        }
    }
}