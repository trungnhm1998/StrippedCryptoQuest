using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.Items;
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
        void InitEquipments(ICharacter character);
    }

    public class EquipmentEffectApplier : MonoBehaviour, IEquipmentEffectApplier
    {
        [SerializeField] private CharacterEquipments _equipments;
        [SerializeField] private InfiniteEffectScriptableObject _equipmentEffectBase;

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
                ApplyEquipmentEffectToCharacter(slot.Equipment);
            }
        }

        /// <summary>
        /// I need to stay consistence with my naming...
        /// </summary>
        private void ClearEquipmentsHandlers() => _equipments.EquipmentAdded -= HandleEquipmentAdded;
        private void RegisterEquipmentHandlers() => _equipments.EquipmentAdded += HandleEquipmentAdded;
        private void HandleEquipmentAdded(EquipmentInfo equipment) => ApplyEquipmentEffectToCharacter(equipment);

        private void ApplyEquipmentEffectToCharacter(EquipmentInfo equipment)
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