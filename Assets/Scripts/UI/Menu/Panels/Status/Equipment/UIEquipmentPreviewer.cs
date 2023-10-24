using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Hero;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Menu.Panels.Status.Stats;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
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
        private HeroBehaviour _inspectingHero;

        private void OnDestroy()
        {
            if (_cloneBehaviour == null) return;
            _cloneBehaviour.AttributeSystem.PostAttributeChange -= PreviewStats;
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
            _inspectingHero = inspectingHero;
            ResetAttributesUI();

            if (inspectingHero.IsValid() == false || !equipment.IsValid()) return;
            if (!equipment.IsCompatibleWithHero(inspectingHero)) return;

            SetupCloneCharacter(inspectingHero);
            EquipEquipmentToMannequin(equipment, equippingSlot);
        }

        public void PreviewUnequipEquipment(EquipmentSlot.EType equippingSlot, HeroBehaviour inspectingHero)
        {
            _inspectingHero = inspectingHero;
            ResetAttributesUI();

            if (inspectingHero.IsValid() == false) return;

            SetupCloneCharacter(inspectingHero);
            UnEquipEquipmentToMannequin(equippingSlot);
        }

        private void PreviewAttributeChange(HeroBehaviour inspectingHero)
        {
            if (inspectingHero == null || inspectingHero.IsValid() == false) return;
            inspectingHero.TryGetComponent<AttributeSystemBehaviour>(out var current);
            List<AttributeValue> currentValues = new(current.AttributeValues);

            _cloneBehaviour.TryGetComponent<AttributeSystemBehaviour>(out var cloned);
            List<AttributeValue> afterValues = new(cloned.AttributeValues);

            PreviewValue(currentValues, afterValues);
        }

        private void SetupCloneCharacter(HeroBehaviour inspectingHero)
        {
            if (IsNeedToCloneCharacter(inspectingHero))
            {
                CloneHero(inspectingHero);
            }

            UpdateEquipment(inspectingHero);
        }

        private bool IsNeedToCloneCharacter(HeroBehaviour inspectingHero) =>
            _cloneCharacter == null || _cloneBehaviour.Spec.Id != inspectingHero.Spec.Id;

        private void UpdateEquipment(HeroBehaviour inspectingHero)
        {
            var clonedEquipmentsController = _cloneBehaviour.GetComponent<EquipmentsController>();

            clonedEquipmentsController.UnequipAll();

            var slots = inspectingHero.GetEquipments().Slots;

            foreach (var slot in slots)
            {
                clonedEquipmentsController.Equip(slot.Equipment.Clone(), slot.Type);
            }
        }

        /// <summary>
        /// Clone the character when inspect so we will have a mannequin to apply equipment
        /// </summary>
        /// <param name="hero"></param>
        private void CloneHero(HeroBehaviour hero)
        {
            if (hero.IsValid() == false) return;
            if (_cloneCharacter != null)
            {
                _cloneBehaviour.AttributeSystem.PostAttributeChange -= PreviewStats;
                Destroy(_cloneCharacter);
            }

            _cloneCharacter = Instantiate(hero.gameObject, transform);
            // TODO: REFACTOR EQUIPMENTS
            _cloneBehaviour = _cloneCharacter.GetComponent<HeroBehaviour>();
            _cloneBehaviour.AttributeSystem.PostAttributeChange += PreviewStats;

            _cloneBehaviour.Init(new HeroSpec()
            {
                Equipments = new Equipments()
                {
                    Slots = new()
                },
                Unit = hero.Spec.Unit,
                Experience = hero.Spec.Experience
            });
        }

        private void PreviewStats(AttributeScriptableObject attribute, AttributeValue oldVal, AttributeValue newVal) =>
            PreviewAttributeChange(_inspectingHero);

        /// <summary>
        /// Equip the equipment to the mannequin
        /// </summary>
        private void EquipEquipmentToMannequin(EquipmentInfo inspectingEquipment, EquipmentSlot.EType equippingSlot)
        {
            _cloneBehaviour.TryGetComponent<EquipmentsController>(out var clonedEquipmentsController);
            clonedEquipmentsController.Equip(inspectingEquipment.Clone(), equippingSlot);
        }

        private void UnEquipEquipmentToMannequin(EquipmentSlot.EType equippingSlot)
        {
            _cloneBehaviour.TryGetComponent<EquipmentsController>(out var clonedEquipmentsController);
            clonedEquipmentsController.Unequip(equippingSlot);
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

        public void ResetAttributesUI()
        {
            for (var i = 0; i < _uiAttributes.Length; i++)
            {
                _uiAttributes[i].ResetAttributeUI();
            }
        }
    }
}