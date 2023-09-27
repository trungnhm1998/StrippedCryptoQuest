using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Menu.Panels.Status.Stats;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UIEquipmentPreviewer : MonoBehaviour
    {
        [SerializeField] private UIEquipmentsInventory _equipmentsInventory;
        [SerializeField] private UIStatusCharacter _characterStatusUI;
        [SerializeField] private UIAttribute[] _uiAttributes;

        private GameObject _cloneCharacterGO;
        private CharacterBehaviourBase _clonedCharacter;

        private void OnEnable()
        {
            _equipmentsInventory.InspectingEquipment += PreviewEquipment;
            _characterStatusUI.InspectingCharacter += CloneHero;
        }

        private void OnDisable()
        {
            _equipmentsInventory.InspectingEquipment -= PreviewEquipment;
            _characterStatusUI.InspectingCharacter -= CloneHero;
        }

        /// <summary>
        /// Clone the character when inspect so we will have a mannequin to apply equipment
        /// </summary>
        /// <param name="hero"></param>
        private void CloneHero(HeroBehaviour hero)
        {
            return;
            if (hero.IsValid() == false) return;
            if (_cloneCharacterGO != null) Destroy(_cloneCharacterGO);

            _cloneCharacterGO = Instantiate(hero.gameObject, transform);
            // TODO: REFACTOR EQUIPMENTS
            _clonedCharacter = _cloneCharacterGO.GetComponent<CharacterBehaviourBase>();
            _clonedCharacter.Init(_clonedCharacter.Spec);
        }

        private void CloneEquipingStatus(EquipmentInfo inspectingEquipment, HeroBehaviour inspectingChar)
        {
            // TODO: REFACTOR EQUIPMENTS
            // if (_clonedCharacter == null) return;
            // var originalEquipments = inspectingChar.Equipments.Slots;
            // var clonedEquipments = _clonedCharacter.Spec.Equipments;
            // var clonedEquipmentSlots = clonedEquipments.Slots;
            //
            // foreach (var equipment in clonedEquipmentSlots.ToList())
            // {
            //     clonedEquipments.Unequip(equipment.Equipment);
            // }
            //
            // foreach (var equipment in originalEquipments)
            // {
            //     clonedEquipments.Equip(equipment.Equipment.Clone());
            // }
            //
            // _clonedCharacter.EffectSystem.UpdateAttributeModifiersUsingAppliedEffects();
        }

        /// <summary>
        /// Check whether we should equip or unequip to preview
        /// If inspecting the equipping equipment, preview unequip
        /// else equip to preview
        /// </summary>
        /// <param name="inspectingEquipment"></param>
        /// <param name="inspectingHero"></param>
        private void CheckEquipInspect(EquipmentInfo inspectingEquipment, HeroBehaviour inspectingHero)
        {
            var clonedCharacterSpec = _clonedCharacter.Spec;
            var clonedEquipmentsBehaviour = clonedCharacterSpec.Equipments;
            var clonedEquipments = clonedCharacterSpec.Equipments.Slots;
            for (var index = 0; index < clonedEquipments.Count; index++)
            {
                var equipment = clonedEquipments[index];
                if (inspectingEquipment.Equals(equipment.Equipment))
                {
                    clonedEquipmentsBehaviour.Unequip(equipment.Equipment);
                    return;
                }
            }

            clonedEquipmentsBehaviour.Equip(inspectingEquipment.Clone());
        }

        /// <summary>
        /// Reset atrtibutes UI and Clone the equipping status
        /// Then Equip the clone equipment to preview 
        /// </summary>
        /// <param name="equipment">to preview stats</param>
        /// <param name="inspectingHero">on which character</param>
        private void PreviewEquipment(EquipmentInfo equipment, HeroBehaviour inspectingHero)
        {
            ResetAttributesUI();

            if (inspectingHero.IsValid() == false || !equipment.IsValid()) return;

            if (_cloneCharacterGO == null)
            {
                CloneHero(inspectingHero);
            }
            Debug.Log($"UIEquipmentsInventory::PreviewEquipmentStats {equipment}");
            var clonedAttributeSystem = _clonedCharacter.AttributeSystem;

            CloneEquipingStatus(equipment, inspectingHero);
            List<AttributeValue> currentValues = new(clonedAttributeSystem.AttributeValues);

            CheckEquipInspect(equipment, inspectingHero);
            _clonedCharacter.EffectSystem.UpdateAttributeModifiersUsingAppliedEffects();

            List<AttributeValue> afterValues = new(clonedAttributeSystem.AttributeValues);
            if (equipment.IsCompatibleWithHero(inspectingHero))
            {
                PreviewValue(currentValues, afterValues);
            }
        }

        private void PreviewValue(List<AttributeValue> currentValues, List<AttributeValue> afterValues)
        {
            var clonedAttributeSystem = _clonedCharacter.AttributeSystem;

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

        public void ResetAttributesUI()
        {
            for (var i = 0; i < _uiAttributes.Length; i++)
            {
                _uiAttributes[i].ResetAttributeUI();
            }
        }
    }
}