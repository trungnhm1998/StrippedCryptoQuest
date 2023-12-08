using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using UnityEngine;

namespace CryptoQuest.UI.Menu
{
    public class UIEquipmentPreviewer : MonoBehaviour
    {
        /// <summary>
        /// Change the UI when inspecting the equipment
        /// </summary>
        [SerializeField] private UIAttribute[] _uiAttributes;

        /// <summary>
        /// Try equip the equipment and unequip after preview
        /// <para>If you listening to <see cref="EquipmentsController"/> events you should remove event 
        /// because this previewer will raise event on Equiped/Unequiped</para>
        /// </summary>
        /// <param name="equipment">to preview stats</param>
        /// <param name="equippingSlot"></param>
        /// <param name="inspectingHero">on which character</param>
        public void PreviewEquipment(Equipment equipment, ESlot equippingSlot,
            HeroBehaviour inspectingHero)
        {
            ResetAttributesUI();

            if (!inspectingHero.IsValid() || !equipment.IsValid()) return;
            if (!inspectingHero.TryGetComponent<EquipmentsController>(out var equipmentController))
                return;

            var attributeSystem = inspectingHero.AttributeSystem;
            var equippingEquipment = equipmentController.GetEquipmentInSlot(equippingSlot);

            List<AttributeValue> currentValues = new(attributeSystem.AttributeValues);

            equipmentController.Equip(equipment, equippingSlot);
            List<AttributeValue> afterValues = new(attributeSystem.AttributeValues);

            // re-equipping the equipment
            equipmentController.Unequip(equipment);
            if (equippingEquipment != null && equippingEquipment.IsValid())
                equipmentController.Equip(equippingEquipment, equippingSlot);

            PreviewValue(inspectingHero, currentValues, afterValues);
        }

        /// <summary>
        /// Try preview if unequip equipment
        /// <para>If you listening to <see cref="EquipmentsController"/> events you should remove event 
        /// because this previewer will raise event on Equiped/Unequiped</para>
        /// </summary>
        /// <param name="equipment">to preview stats</param>
        /// <param name="equippingSlot"></param>
        /// <param name="inspectingHero">on which character</param>
        public void PreviewUnequipEquipment(Equipment equipment, ESlot equippingSlot,
            HeroBehaviour inspectingHero)
        {
            ResetAttributesUI();

            if (!inspectingHero.IsValid() || !equipment.IsValid()) return;
            if (!inspectingHero.TryGetComponent<EquipmentsController>(out var equipmentController))
                return;

            var attributeSystem = inspectingHero.AttributeSystem;

            List<AttributeValue> currentValues = new(attributeSystem.AttributeValues);

            equipmentController.Unequip(equippingSlot);
            List<AttributeValue> afterValues = new(attributeSystem.AttributeValues);

            equipmentController.Equip(equipment, equippingSlot);

            PreviewValue(inspectingHero, currentValues, afterValues);
        }

        private void PreviewValue(HeroBehaviour inspectingHero,
            List<AttributeValue> currentValues, List<AttributeValue> afterValues)
        {
            var attributeSystem = inspectingHero.AttributeSystem;
            var indexCache = attributeSystem.GetAttributeIndexCache();

            for (int i = 0; i < _uiAttributes.Length; i++)
            {
                var uiAttribute = _uiAttributes[i];
                var attributeIndex = indexCache[uiAttribute.Attribute];

                var currentVal = currentValues[attributeIndex].CurrentValue;
                var afterVal = afterValues[attributeIndex].CurrentValue;

                uiAttribute.CompareValue(currentVal, afterVal);
            }
        }

        public void ResetAttributesUI()
        {
            for (var i = 0; i < _uiAttributes.Length; i++)
            {
                _uiAttributes[i].ResetAttributeUI();
            }
        }
    }
}