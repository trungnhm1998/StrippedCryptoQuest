using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;
using UnityEngine;
using CryptoQuest.Gameplay.Helper;

namespace CryptoQuest.Gameplay
{
    public class EquipmentEffectApplier : MonoBehaviour, ICharacterComponent
    {
        [SerializeField] private CharacterEquipments _equipments;
        [SerializeField] private InfiniteEffectScriptableObject _equipmentEffectBase;

        private readonly ILevelAttributeCalculator _equipmentAttributeCalculator = new DefaultLevelAttributeCalculator();
        private CharacterBehaviourBase _character;


        public void Init(CharacterBehaviourBase characterBehaviourBase)
        {
            InitEquipments(characterBehaviourBase);
        }

        /// <summary>
        /// Find all <see cref="EquipmentInfo"/> in <see cref="CharacterEquipments"/> then create and apply effect to character
        /// </summary>
        private void InitEquipments(CharacterBehaviourBase character)
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
            var activeEffectSpec = _character.ApplyEffect(CreateEffectSpecFromEquipment(equipment));
            equipment.SetActiveEffectSpec(activeEffectSpec);
        }

        private void CreateAndSetEffectDefToEquipment(EquipmentInfo equipment)
        {
            var equipmentEffectDef = CreateEffectDefFormEquipment(equipment);
            equipment.EffectDef = equipmentEffectDef;
        }

        private InfiniteEffectScriptableObject CreateEffectDefFormEquipment(EquipmentInfo equipment)
        {
            var attributes = equipment.Stats.Attributes;
            var equipmentEffectDef = Instantiate(_equipmentEffectBase); // Using preconfigured effect base

            var modifiers = new EffectAttributeModifier[attributes.Length];
            for (int i = 0; i < attributes.Length; i++)
            {
                var attribute = attributes[i];
                // TODO: Use magnitude calculation here
                var attributeValue =
                    _equipmentAttributeCalculator.GetValueAtLevel(equipment.Level, attribute, equipment.Stats.MaxLevel);

                modifiers[i] = new EffectAttributeModifier
                {
                    Attribute = attribute.Attribute,
                    ModifierType = EAttributeModifierType.Add,
                    Value = attributeValue
                };
            }

            equipmentEffectDef.EffectDetails.Modifiers = modifiers;
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
                : _character.GameplayAbilitySystem.MakeOutgoingSpec(equipment.EffectDef);
        }
    }
}