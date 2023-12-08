using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Item.Equipment;
using IndiGames.Core.Common;

namespace CryptoQuest.Battle.Components
{
    public class InventoryEquipmentsController : CharacterComponentBase
    {
        private EquipmentsController _equipmentsController;
        private IInventoryController _inventoryController;

        protected override void OnInit()
        {
            _inventoryController ??= ServiceProvider.GetService<IInventoryController>();
            _equipmentsController = Character.GetComponent<EquipmentsController>();

            _equipmentsController.Equipped += RemoveFromInventory;
            _equipmentsController.Removed += AddToInventory;
        }

        protected override void OnReset()
        {
            _equipmentsController.Equipped -= RemoveFromInventory;
            _equipmentsController.Removed -= AddToInventory;
        }

        private void RemoveFromInventory(IEquipment equipment) => _inventoryController.Remove(equipment);

        private void AddToInventory(IEquipment equipment) => _inventoryController.Add(equipment);
    }
}