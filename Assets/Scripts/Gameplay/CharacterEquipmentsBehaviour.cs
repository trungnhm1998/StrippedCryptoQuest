using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public interface IEquipmentEffector
    {
        void InitEquipments(ICharacter character);
        void ApplyEffect(EquipmentInfo equipment);
    }

    public class CharacterEquipmentsBehaviour : MonoBehaviour, IEquipmentEffector
    {
        private struct EquipmentEffectContainer
        {
            public InfiniteEffectScriptableObject EffectDef;
            public ActiveEffectSpecification ActiveEffectSpec;
        }

        [SerializeField] private CharacterEquipments _equipments;
        [field: SerializeField] private InfiniteEffectScriptableObject _equipmentEffectBase;
        private Dictionary<string, EquipmentEffectContainer> _equipmentEffects = new();

        private ICharacter _character;

        public void InitEquipments(ICharacter character)
        {
            _character = character;
            ClearEquipmentsHandlers();
            _equipments = _character.Spec.Equipments;
            RegisterEquipmentHandlers();

            foreach (var slot in _equipments.Slots)
            {
                if (slot.IsValid() == false) continue;

                var equipment = slot.Equipment;
                ApplyEffect(equipment);
            }

            _character.UpdateAttributeValues();
        }

        /// <summary>
        /// I need to stay consistence with my naming...
        /// </summary>
        private void ClearEquipmentsHandlers()
        {
            _equipments.EquipmentAdded -= HandleEquipmentAdded;
            // _equipments.EquipmentRemoved -= HandleEquipmentRemoved;
        }

        private void RegisterEquipmentHandlers()
        {
            _equipments.EquipmentAdded += HandleEquipmentAdded;
            // _equipments.EquipmentRemoved += HandleEquipmentRemoved;
        }

        private void HandleEquipmentAdded(EquipmentSlot.EType slotType, EquipmentSlot slot)
        {
            ApplyEffect(slot.Equipment);
            _character.UpdateAttributeValues();
        }

        public void ApplyEffect(EquipmentInfo equipment)
        {
            if (equipment.IsValid() == false) return;

            var attributes = equipment.Stats.Attributes;
            var equipmentEffectDef = Instantiate(_equipmentEffectBase);

            var modifiers = new EffectAttributeModifier[attributes.Length];
            for (int i = 0; i < attributes.Length; i++)
            {
                var attribute = attributes[i];
                modifiers[i] = new EffectAttributeModifier
                {
                    Attribute = attribute.Attribute,
                    ModifierType = EAttributeModifierType.Add,
                    // TODO: https://github.com/indigames/CryptoQuestClient/issues/1045 Implement GetValueAtLevel like CharacterStatsInitializer
                    Value = attribute.MinValue
                };
            }

            _equipmentEffectBase.EffectDetails.Modifiers = modifiers;

            var effectSpec = _character.GameplayAbilitySystem.MakeOutgoingSpec(_equipmentEffectBase);
            var activeEffectSpec = _character.ApplyEffect(effectSpec);

            var equipmentEffectContainer = new EquipmentEffectContainer()
            {
                EffectDef = equipmentEffectDef,
                ActiveEffectSpec = activeEffectSpec
            };

            if (_equipmentEffects.TryAdd(equipment.Id, equipmentEffectContainer)) return;
            Debug.LogWarning($"Something went wrong when adding equipment {equipment} to character {_character}");
        }
    }
}