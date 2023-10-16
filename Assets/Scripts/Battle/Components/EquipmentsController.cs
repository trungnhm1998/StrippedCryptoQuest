using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Item.Equipment;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;
using ESlotType =
    CryptoQuest.Item.Equipment.EquipmentSlot.EType;
using ECategory = CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type.EEquipmentCategory;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;

namespace CryptoQuest.Battle.Components
{
    internal interface IEquipmentsProvider
    {
        Equipments GetEquipments();
    }

    public class EquipmentsController : CharacterComponentBase
    {
        public event Action<EquipmentInfo> Equipped;
        public event Action<EquipmentInfo> Removed;
        private IEquipmentsProvider _equipmentsProvider;
        private HeroBehaviour _hero;
        private Equipments _equipments;
        public Equipments Equipments => _equipments;

        public List<EquipmentSlot> Slots => _equipments.Slots;

        protected override void Awake()
        {
            base.Awake();
            _equipmentsProvider = GetComponent<IEquipmentsProvider>();
            _hero = GetComponent<HeroBehaviour>();
        }

        public override void Init()
        {
            _equipments = _equipmentsProvider.GetEquipments();
            StartCoroutine(InitEquipments());
        }

        /// <summary>
        /// Find all <see cref="EquipmentInfo"/> in <see cref="Equipments"/> then create and apply effect to character
        /// </summary>
        private IEnumerator InitEquipments()
        {
            var equipmentDefProvider = ServiceProvider.GetService<IEquipmentDefProvider>();
            foreach (var equipmentSlot in _equipments.Slots.ToList())
            {
                if (equipmentSlot.IsValid())
                    yield return equipmentDefProvider.Load(equipmentSlot.Equipment);
            }

            var equipmentSlots = new List<EquipmentSlot>(_equipments.Slots);
            for (var index = 0; index < equipmentSlots.Count; index++)
            {
                var slot = equipmentSlots[index];
                if (slot.IsValid() == false) continue;
                ApplyEquipmentEffectToCharacter(slot.Equipment);
            }
        }

        private void RemoveEquipmentEffectFromCharacter(EquipmentInfo equipment)
        {
            if (equipment.activeGameplayEffect == null) return;
            Character.RemoveEffect(equipment.activeGameplayEffect.Spec); // TODO: REFACTOR
        }

        private void CreateAndSetEffectDefToEquipment(EquipmentInfo equipment)
        {
            var equipmentEffectDef = CreateEffectDefFormEquipment(equipment);
            equipment.EffectDef = equipmentEffectDef;
        }

        private GameplayEffectDefinition CreateEffectDefFormEquipment(EquipmentInfo equipment)
        {
            var attributes = equipment.Stats;
            var equipmentEffectDef = ScriptableObject.CreateInstance<GameplayEffectDefinition>();
            equipmentEffectDef.Policy = new InfinitePolicy();

            var modifiers = new EffectAttributeModifier[attributes.Length];
            for (int i = 0; i < attributes.Length; i++)
            {
                var attribute = attributes[i];
                var attributeValue = (equipment.Level - 1) * equipment.ValuePerLvl;

                modifiers[i] = new EffectAttributeModifier
                {
                    Attribute = attribute.Attribute,
                    OperationType = EAttributeModifierOperationType.Add,
                    Value = attribute.Value + attributeValue
                };
            }

            equipmentEffectDef.EffectDetails = new EffectDetails()
            {
                Modifiers = modifiers
            };
            return equipmentEffectDef;
        }

        /// <summary>
        /// Create a <see cref="GameplayEffectSpec"/> using the character
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns>A gameplay spec that can be use to apply into the system</returns>
        private GameplayEffectSpec CreateEffectSpecFromEquipment(EquipmentInfo equipment)
        {
            return equipment.IsValid() == false
                ? new GameplayEffectSpec()
                : Character.AbilitySystem.MakeOutgoingSpec(equipment.EffectDef);
        }

        public EquipmentInfo GetEquipmentInSlot(ESlotType slotType)
        {
            foreach (var equipmentSlot in _equipments.Slots)
            {
                if (equipmentSlot.Type == slotType)
                    return equipmentSlot.Equipment;
            }

            Debug.Log($"No slot {slotType} found, create new slot");
            var slot = new EquipmentSlot()
            {
                Type = slotType,
                Equipment = new EquipmentInfo()
            };
            _equipments.Slots.Add(slot);
            return slot.Equipment;
        }

        /// <summary>
        /// <para>First check if this equipment allow to equip in this slot</para>
        /// 
        /// <para>Then find all required slots for this equipment
        /// remove all equipment that in the required slots and put in back to inventory
        /// equip the equipment
        /// the required slots will now occupied by the same equipment (check GUID)
        /// apply the effect</para>
        /// </summary>
        public void Equip(EquipmentInfo equipmentInfo, ESlotType equippingSlot)
        {
            if (equipmentInfo.IsValid() == false) return;
            var allowedSlots = equipmentInfo.AllowedSlots;
            if (allowedSlots.Contains(equippingSlot) == false) return;
            var requiredSlots = equipmentInfo.RequiredSlots;
            Unequip(GetEquipmentInSlot(equippingSlot));
            OnEquipmentAdded(equipmentInfo, equippingSlot, requiredSlots);
        }

        private void OnEquipmentAdded(EquipmentInfo equipment, ESlotType equippingSlot, ESlotType[] requiredSlots)
        {
            SetEquipmentInSlot(equipment, equippingSlot);
            foreach (var slot in requiredSlots)
                if (equippingSlot != slot)
                    SetEquipmentInSlot(equipment, slot);

            equipment.Equipped(_hero.Spec.Id);
            ApplyEquipmentEffectToCharacter(equipment);
            Equipped?.Invoke(equipment);
        }

        private void ApplyEquipmentEffectToCharacter(EquipmentInfo equipment)
        {
            // TODO: Should I allow equip dead character?
            if (_hero.IsValid() == false || equipment.IsValid() == false)
                return;

            // Code smell here
            CreateAndSetEffectDefToEquipment(equipment);
            var activeEffectSpec = Character.ApplyEffect(CreateEffectSpecFromEquipment(equipment));
            equipment.SetActiveEffectSpec(activeEffectSpec);
        }

        public void Unequip(ESlotType slotToUnequip)
        {
            var equipment = GetEquipmentInSlot(slotToUnequip);
            Unequip(equipment);
        }

        /// <summary>
        /// Unequip the equipment in all required slots, and raise an event,
        /// InventoryController, or some manager should listen to this event and add the equipment back to inventory
        /// </summary>
        /// <param name="equipment">This equipment should already in <see cref="Slots"/></param>
        public void Unequip(EquipmentInfo equipment)
        {
            if (equipment.IsValid() == false) return;
            for (var index = 0; index < _equipments.Slots.Count; index++)
            {
                var slot = _equipments.Slots[index];
                if (slot.IsValid()
                    // This handle the case when the equipment is in multiple slots
                    && slot.Equipment == equipment)
                {
                    SetEquipmentInSlot(new EquipmentInfo(), slot.Type);
                }
            }

            OnEquipmentRemoved(equipment);
        }

        private void OnEquipmentRemoved(EquipmentInfo equipment)
        {
            // EquipmentRemoved?.Invoke(equipment, equippedSlots);
            RemoveEquipmentEffectFromCharacter(equipment);
            equipment.UnEquipped();
            Removed?.Invoke(equipment);
        }

        private void SetEquipmentInSlot(EquipmentInfo equipment, ESlotType slotType)
        {
            for (var index = 0; index < Slots.Count; index++)
            {
                var slot = Slots[index];
                if (slot.Type == slotType)
                {
                    slot.Equipment = equipment;
                    Slots[index] = slot;
                    return;
                }
            }

            var equipmentSlot = new EquipmentSlot()
            {
                Equipment = equipment,
                Type = slotType
            };
            Slots.Add(equipmentSlot);
        }
    }
}