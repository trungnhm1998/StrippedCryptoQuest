using System.Collections.Generic;
using CryptoQuest.Character.Hero;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.Battle.Components
{
    public class EquipmentsEffectInitializer : CharacterComponentBase
    {
        private EquipmentsEffectController _equipmentsEffectController;

        protected override void OnInit()
        {
            _equipmentsEffectController = Character.GetComponent<EquipmentsEffectController>();
            ApplyEffectFromEquippingItems();
        }

        /// <summary>
        /// Find all <see cref="EquipmentInfo"/> in <see cref="Equipments"/> then create and apply effect to character
        /// </summary>
        private void ApplyEffectFromEquippingItems()
        {
            var equipmentsProvider = Character.GetComponent<IEquipmentsProvider>();
            var equipments = equipmentsProvider.GetEquipments();
            var equipmentSlots = new List<EquipmentSlot>(equipments.Slots);
            foreach (var slot in equipmentSlots)
            {
                if (slot.IsValid() == false) continue;
                _equipmentsEffectController.ApplyEffect(slot.Equipment);
            }
        }

        protected override void OnReset() { }
    }
}