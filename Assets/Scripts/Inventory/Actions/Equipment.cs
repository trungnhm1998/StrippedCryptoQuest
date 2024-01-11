using CryptoQuest.Item.Equipment;
using IndiGames.Core.Events;

namespace CryptoQuest.Inventory.Actions
{
    public class EquipmentActionBase : ActionBase
    {
        public IEquipment Item { get; }

        protected EquipmentActionBase(IEquipment item)
        {
            Item = item;
        }
    }

    public class AddEquipmentAction : EquipmentActionBase
    {
        public AddEquipmentAction(IEquipment item) : base(item) { }
    }

    public class RemoveEquipmentAction : EquipmentActionBase
    {
        public RemoveEquipmentAction(IEquipment item) : base(item) { }
    }

    public class AddEquipmentRequestAction : ActionBase
    {
        public string EquipmentId;

        public AddEquipmentRequestAction(string equipmentId)
        {
            EquipmentId = equipmentId;
        }
    }
}