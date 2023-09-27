using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Menu.Panels.Status.Stats;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UIEquipmentPreviewer : MonoBehaviour
    {
        /// <summary>
        /// Change the UI when inspecting the equipment
        /// </summary>
        [SerializeField] private UIAttribute[] _uiAttributes;

        private GameObject _cloneCharacter;
        private HeroBehaviour _cloneBehaviour;

        /// <summary>
        /// Check whether we should equip or unequip to preview
        /// If inspecting the equipping equipment, preview unequip
        /// else equip to preview
        /// </summary>
        /// <param name="inspectingEquipment"></param>
        /// <param name="inspectingHero"></param>
        private void CheckEquipInspect(EquipmentInfo inspectingEquipment, HeroBehaviour inspectingHero)
        {
            // var clonedCharacterSpec = _cloneBehaviour.Spec;
            // var clonedEquipmentsBehaviour = clonedCharacterSpec.Equipments;
            // var clonedEquipments = clonedCharacterSpec.Equipments.Slots;
            // for (var index = 0; index < clonedEquipments.Count; index++)
            // {
            //     var equipment = clonedEquipments[index];
            //     if (inspectingEquipment.Equals(equipment.Equipment))
            //     {
            //         clonedEquipmentsBehaviour.Unequip(equipment.Equipment);
            //         return;
            //     }
            // }
            //
            // clonedEquipmentsBehaviour.Equip(inspectingEquipment.Clone());
        }

        /// <summary>
        /// Reset attributes UI and Clone the equipping status
        /// Then Equip the clone equipment to preview 
        /// </summary>
        /// <param name="equipment">to preview stats</param>
        /// <param name="equippingSlot"></param>
        /// <param name="inspectingHero">on which character</param>
        public void PreviewEquipment(EquipmentInfo equipment, EquipmentSlot.EType equippingSlot,
            HeroBehaviour inspectingHero)
        {
            if (inspectingHero.IsValid() == false || !equipment.IsValid()) return;

            ResetAttributesUI();

            if (_cloneCharacter == null)
                CloneHero(inspectingHero);

            Debug.Log($"UIEquipmentsInventory::PreviewEquipmentStats {equipment}");
            var clonedAttributeSystem = _cloneBehaviour.GetComponent<AttributeSystemBehaviour>();
            List<AttributeValue> currentValues = new(clonedAttributeSystem.AttributeValues);
            EquipEquipmentToMannequin(equipment, equippingSlot);

            CheckEquipInspect(equipment, inspectingHero);
            _cloneBehaviour.GetComponent<EffectSystemBehaviour>().UpdateAttributeModifiersUsingAppliedEffects();

            List<AttributeValue> afterValues = new(clonedAttributeSystem.AttributeValues);
            if (equipment.IsCompatibleWithHero(inspectingHero))
            {
                PreviewValue(currentValues, afterValues);
            }
        }

        public void PreviewUnequipEquipment(EquipmentInfo equipment, HeroBehaviour inspectingHero)
        {
            if (inspectingHero.IsValid() == false || !equipment.IsValid()) return;
            ResetAttributesUI();
            CloneHero(inspectingHero);
            var clonedAttributeSystem = _cloneBehaviour.GetComponent<AttributeSystemBehaviour>();
            List<AttributeValue> currentValues = new(clonedAttributeSystem.AttributeValues);
            var clonedEquipmentsController = _cloneBehaviour.GetComponent<EquipmentsController>();
            clonedEquipmentsController.Unequip(equipment);
            _cloneBehaviour.GetComponent<EffectSystemBehaviour>().UpdateAttributeModifiersUsingAppliedEffects();
            List<AttributeValue> afterValues = new(clonedAttributeSystem.AttributeValues);
            PreviewValue(currentValues, afterValues);
        }

        /// <summary>
        /// Clone the character when inspect so we will have a mannequin to apply equipment
        /// </summary>
        /// <param name="hero"></param>
        private void CloneHero(HeroBehaviour hero)
        {
            if (hero.IsValid() == false) return;
            if (_cloneCharacter != null) Destroy(_cloneCharacter);

            _cloneCharacter = Instantiate(hero.gameObject, transform);
            // TODO: REFACTOR EQUIPMENTS
            _cloneBehaviour = _cloneCharacter.GetComponent<HeroBehaviour>();
            var slots = hero.GetEquipments().Slots;
            List<EquipmentSlot> cloneSlots = new();
            for (var index = 0; index < slots.Count; index++)
            {
                var slot = slots[index];
                cloneSlots.Add(new EquipmentSlot()
                {
                    Type = slot.Type,
                    Equipment = slot.Equipment.Clone()
                });
            }

            _cloneBehaviour.Init(new HeroSpec()
            {
                Equipments = new Equipments()
                {
                    Slots = cloneSlots
                },
                Unit = hero.Spec.Unit,
                Experience = hero.Spec.Experience
            });
        }

        /// <summary>
        /// Equip the equipment to the mannequin
        /// </summary>
        private void EquipEquipmentToMannequin(EquipmentInfo inspectingEquipment, EquipmentSlot.EType equippingSlot)
        {
            var clonedEquipmentsController = _cloneBehaviour.GetComponent<EquipmentsController>();
            clonedEquipmentsController.Equip(inspectingEquipment.Clone(), equippingSlot);
            _cloneBehaviour.GetComponent<EffectSystemBehaviour>().UpdateAttributeModifiersUsingAppliedEffects();
        }

        private void PreviewValue(List<AttributeValue> currentValues, List<AttributeValue> afterValues)
        {
            var clonedAttributeSystem = _cloneBehaviour.GetComponent<AttributeSystemBehaviour>();

            var indexCache = clonedAttributeSystem.GetAttributeIndexCache();
            for (int i = 0; i < _uiAttributes.Length; i++)
            {
                var uiAttribute = _uiAttributes[i];
                var attributeIndex = indexCache[uiAttribute.Attribute];

                var currentVal = currentValues[attributeIndex].CurrentValue;
                var afterVal = afterValues[attributeIndex].CurrentValue;

                uiAttribute.CompareValue(currentVal, afterVal);
            }
        }

        private void ResetAttributesUI()
        {
            for (var i = 0; i < _uiAttributes.Length; i++)
            {
                _uiAttributes[i].ResetAttributeUI();
            }
        }
    }
}