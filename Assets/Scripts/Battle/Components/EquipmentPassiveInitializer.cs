using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.Battle.Components
{
    public class EquipmentPassiveInitializer : CharacterComponentBase
    {
        private EquipmentPassiveController _equipmentsPassiveController;
        private List<IEquipment> _equippingItems = new();

        protected override void OnInit()
        {
            _equipmentsPassiveController = Character.GetComponent<EquipmentPassiveController>();
            ApplyPassivesFromEquippingItems();
        }

        private void ApplyPassivesFromEquippingItems()
        {
            var equipmentsProvider = Character.GetComponent<IEquipmentsProvider>();
            var equipments = equipmentsProvider.GetEquipments();
            var equipmentSlots = new List<EquipmentSlot>(equipments.Slots);
            SortAndCacheEquippingItems(equipmentSlots);

            foreach (var equippingItem in _equippingItems)
            {
                if (!equippingItem.IsValid()) continue;
                _equipmentsPassiveController.GrantPassive(equippingItem);
            }
        }

        private void SortAndCacheEquippingItems(List<EquipmentSlot> equipmentSlots)
        {
            _equippingItems.Clear();
            foreach (var slot in equipmentSlots)
            {
                if (slot.IsValid() == false) continue;
                if (IsEquippingDuplicateItems(slot.Equipment.Id)) continue;
                _equippingItems.Add(slot.Equipment);
            }
        }

        private bool IsEquippingDuplicateItems(int equipmentId)
        {
            return _equippingItems.Any(item => item.Id == equipmentId);
        }
    }
}