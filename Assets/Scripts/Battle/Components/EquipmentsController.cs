using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Character.Hero;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using UnityEngine;
using ESlotType =
    CryptoQuest.Item.Equipment.EquipmentSlot.EType;
using ECategory = CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type.EEquipmentCategory;

namespace CryptoQuest.Battle.Components
{
    public interface IEquipmentsProvider
    {
        Equipments GetEquipments();
    }

    public class EquipmentsController : CharacterComponentBase
    {
        public event Action<EquipmentInfo> Equipped;
        public event Action<EquipmentInfo> Removed;

        [SerializeField] private EquipmentPrefabDatabase _equipmentPrefabDatabase;

        private IEquipmentsProvider _equipmentsProvider;
        private HeroBehaviour _hero;
        private Equipments _equipments;
        public Equipments Equipments => _equipments;

        public List<EquipmentSlot> Slots => _equipments.Slots;

        /// <summary>
        /// Create an effect based on effect stats
        /// </summary>
        private readonly Dictionary<EquipmentInfo, ActiveGameplayEffect> _equipmentsEffect = new();

        private IInventoryController _inventoryController;

        public override void Init()
        {
            _equipmentsProvider = Character.GetComponent<IEquipmentsProvider>();
            _hero = Character.GetComponent<HeroBehaviour>();
            _equipments = _equipmentsProvider.GetEquipments();
            _inventoryController ??= ServiceProvider.GetService<IInventoryController>();

            foreach (var cache in _equipmentsEffect)
            {
                cache.Value.IsActive = false;
                Character.RemoveEffect(cache.Value.Spec);
            }

            Character.AbilitySystem.AttributeSystem.UpdateAttributeValues();
            _equipmentsEffect.Clear();

            ApplyEffectFromEquippingItems();
        }

        /// <summary>
        /// Find all <see cref="EquipmentInfo"/> in <see cref="Equipments"/> then create and apply effect to character
        /// </summary>
        private void ApplyEffectFromEquippingItems()
        {
            var equipmentSlots = new List<EquipmentSlot>(_equipments.Slots);
            foreach (var slot in equipmentSlots)
            {
                if (slot.IsValid() == false) continue;
                StartCoroutine(LoadAndApplyEffect(slot.Equipment));
            }
        }

        private IEnumerator LoadAndApplyEffect(EquipmentInfo equipment)
        {
            yield return _equipmentPrefabDatabase.LoadDataById(equipment.Data.PrefabId);
            equipment.Config = _equipmentPrefabDatabase.GetDataById(equipment.Data.PrefabId);
            CreateEffectFromEquipmentStatsAndApplyToHero(equipment);
        }

        private void RemoveEquipmentEffectFromCharacter(EquipmentInfo equipment)
        {
            if (_equipmentsEffect.TryGetValue(equipment, out var activeEffect) == false) return;
            _equipmentsEffect.Remove(equipment);
            activeEffect.IsActive = false;
            Character.RemoveEffect(activeEffect.Spec);
            Character.AbilitySystem.AttributeSystem.UpdateAttributeValues();
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
        /// <para>Then find all required slots for this equipment
        /// remove all equipment that in the required slots and put in back to inventory
        /// equip the equipment
        /// the required slots will now occupied by the same equipment (check GUID)
        /// apply the effect</para>
        /// </summary>
        public void Equip(EquipmentInfo equipment, ESlotType equippingSlot)
        {
            if (equipment.IsValid() == false) return;
            var allowedSlots = equipment.AllowedSlots;
            if (allowedSlots.Contains(equippingSlot) == false) return;
            var requiredSlots = equipment.RequiredSlots;
            Unequip(GetEquipmentInSlot(equippingSlot));
            OnEquipmentAdded(equipment, equippingSlot, requiredSlots);
        }

        private void OnEquipmentAdded(EquipmentInfo equipment, ESlotType equippingSlot, ESlotType[] requiredSlots)
        {
            SetEquipmentInSlot(equipment, equippingSlot);
            foreach (var slot in requiredSlots)
                if (equippingSlot != slot)
                    SetEquipmentInSlot(equipment, slot);

            equipment.RemoveFromInventory(_inventoryController);
            CreateEffectFromEquipmentStatsAndApplyToHero(equipment);
            Equipped?.Invoke(equipment);
        }


        private void CreateEffectFromEquipmentStatsAndApplyToHero(EquipmentInfo equipment)
        {
            // TODO: Should I allow equip dead character?
            if (_hero.IsValid() == false || equipment.IsValid() == false) return;

            var activeEffectSpec = Character.ApplyEffect(CreateEffectSpecFromEquipment(equipment));
            _equipmentsEffect.Add(equipment, activeEffectSpec);
        }

        /// <summary>
        /// Create a <see cref="GameplayEffectSpec"/> using the character
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns>A gameplay spec that can be use to apply into the system</returns>
        private GameplayEffectSpec CreateEffectSpecFromEquipment(EquipmentInfo equipment) =>
            Character.AbilitySystem.MakeOutgoingSpec(CreateEffectDefFormEquipment(equipment));

        /// <summary>
        /// Create an infinite effect from the stats, this effect will only expire when the equipment is removed
        /// </summary>
        /// <param name="equipment">The equipment to create effect from</param>
        /// <returns><see cref="GameplayEffectDefinition"/> with <see cref="InfinitePolicy"/> created using Equipment <see cref="Equipment.Stats"/></returns>
        private GameplayEffectDefinition CreateEffectDefFormEquipment(EquipmentInfo equipment)
        {
            var attributes = equipment.Stats;
            var equipmentEffectDef = ScriptableObject.CreateInstance<GameplayEffectDefinition>();
            equipmentEffectDef.Policy = new InfinitePolicy();

            var modifiers = new EffectAttributeModifier[attributes.Length];
            for (var i = 0; i < attributes.Length; i++)
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
            RemoveEquipmentEffectFromCharacter(equipment);
            equipment.AddToInventory(_inventoryController);
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