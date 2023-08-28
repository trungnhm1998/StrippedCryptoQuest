using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;
using UnityEngine;

namespace CryptoQuest.Gameplay
{
    public interface IEquipmentEffectApplier
    {
        /// <summary>
        /// Find all <see cref="EquipmentInfo"/> in <see cref="CharacterEquipments"/> then create and apply effect to character
        /// </summary>
        /// <param name="character"></param>
        void InitEquipments(CharacterBehaviour character);
    }

    public class EquipmentEffectApplier : MonoBehaviour, IEquipmentEffectApplier
    {
        [SerializeField] private CharacterEquipments _equipments;
        [SerializeField] private InfiniteEffectScriptableObject _equipmentEffectBase;

        private CharacterBehaviour _character;

        public void InitEquipments(CharacterBehaviour character)
        {
            _character = character;
            ClearEquipmentsHandlers();
            _equipments = _character.Spec.Equipments;
            RegisterEquipmentHandlers();
            foreach (var slot in _equipments.Slots)
            {
                if (slot.IsValid() == false) continue;
                ApplyEquipmentEffectToCharacter(slot.Equipment, new());
            }
        }

        /// <summary>
        /// I need to stay consistence with my naming...
        /// </summary>
        private void ClearEquipmentsHandlers()
        {
            _equipments.EquipmentAdded -= ApplyEquipmentEffectToCharacter;
            _equipments.EquipmentRemoved -= RemoveEquipmentEffectFromCharacter;
        }

        private void RegisterEquipmentHandlers()
        {
            _equipments.EquipmentAdded += ApplyEquipmentEffectToCharacter;
            _equipments.EquipmentRemoved += RemoveEquipmentEffectFromCharacter;
        }

        private void RemoveEquipmentEffectFromCharacter(EquipmentInfo equipment, List<EquipmentSlot.EType> eTypes)
        {
            _character.RemoveEffect(equipment.ActiveEffect.EffectSpec); // TODO: REFACTOR
        }

        private void ApplyEquipmentEffectToCharacter(EquipmentInfo equipment, List<EquipmentSlot.EType> eTypes)
        {
            if (_character.Spec.IsValid() == false || equipment.IsValid() == false)
                return;

            // Code smell here
            CreateAndSetEffectDefToEquipment(equipment);
            var activeEffectSpec = _character.ApplyEffect(_character.CreateEffectSpecFromEquipment(equipment));
            equipment.SetActiveEffectSpec(activeEffectSpec);
        }

        private void CreateAndSetEffectDefToEquipment(EquipmentInfo equipment)
        {
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

            equipmentEffectDef.EffectDetails.Modifiers = modifiers;
            equipment.EffectDef = equipmentEffectDef;
        }
    }
}