using UnityEngine;
using SlotType = CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquippingSlotContainer.EType;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type
{
    public class DualWieldingTypeSO : EquipmentTypeSO
    {
        public override void Equip(EquipmentInfo equipment, InventorySO inventory)
        {
            base.Equip(equipment, inventory);
        }

        public override void Unequip(InventorySO inventory)
        {
            var isDualWielding = inventory.GetInventorySlot(SlotType.Weapon).Equipment ==
                                 inventory.GetInventorySlot(SlotType.Shield).Equipment;
            if (!isDualWielding)
            {
                Debug.LogError("Trying to unequip dual wielding type, but it's not dual wielding");
                return;
            }

            inventory.Unequip(AllowedSlots[0]);
            inventory.Unequip(AllowedSlots[1]);
        }
    }
}