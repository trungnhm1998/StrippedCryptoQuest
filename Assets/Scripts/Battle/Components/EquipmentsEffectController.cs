﻿using System.Collections.Generic;
using CryptoQuest.Item.Equipment;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class EquipmentsEffectController : CharacterComponentBase
    {
        private EquipmentsController _equipmentsController;
        /// <summary>
        /// Create an effect based on effect stats
        /// </summary>
        private readonly Dictionary<IEquipment, ActiveGameplayEffect> _equipmentsEffect = new();

        public override void Init()
        {
            _equipmentsController = GetComponent<EquipmentsController>();
            _equipmentsController.Equipped += ApplyEffect;
            _equipmentsController.Removed += RemoveEffect;

            ClearEffects();
        }

        protected override void OnReset()
        {
            _equipmentsController.Equipped -= ApplyEffect;
            _equipmentsController.Removed -= RemoveEffect;
            
            ClearEffects();
        }

        private void ClearEffects()
        {
            foreach (var effect in _equipmentsEffect)
            {
                effect.Value.IsActive = false;
                Character.RemoveEffect(effect.Value.Spec);
            }

            _equipmentsEffect.Clear();
            Character.AbilitySystem.AttributeSystem.UpdateAttributeValues();
        }

        public void ApplyEffect(IEquipment equipment)
        {
            // TODO: Should I allow equip dead character?
            if (equipment.IsValid() == false) return;
            if (_equipmentsEffect.ContainsKey(equipment)) return; // two handed weapon will apply effect twice

            var activeEffectSpec = Character.ApplyEffect(CreateEffectSpecFromEquipment(equipment));
            _equipmentsEffect.Add(equipment, activeEffectSpec);
        }

        /// <summary>
        /// Create a <see cref="GameplayEffectSpec"/> using the character
        /// </summary>
        /// <param name="equipment"></param>
        /// <returns>A gameplay spec that can be use to apply into the system</returns>
        private GameplayEffectSpec CreateEffectSpecFromEquipment(IEquipment equipment) =>
            Character.AbilitySystem.MakeOutgoingSpec(CreateEffectDefFormEquipment(equipment));

        /// <summary>
        /// Create an infinite effect from the stats, this effect will only expire when the equipment is removed
        /// </summary>
        /// <param name="equipment">The equipment to create effect from</param>
        /// <returns><see cref="GameplayEffectDefinition"/> with <see cref="InfinitePolicy"/> created using Equipment <see cref="Equipment.Stats"/></returns>
        private GameplayEffectDefinition CreateEffectDefFormEquipment(IEquipment equipment)
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

        public void RemoveEffect(IEquipment equipment)
        {
            if (_equipmentsEffect.TryGetValue(equipment, out var activeEffect) == false) return;
            _equipmentsEffect.Remove(equipment);
            activeEffect.IsActive = false;
            Character.RemoveEffect(activeEffect.Spec);
            Character.AbilitySystem.AttributeSystem.UpdateAttributeValues();
        }
    }
}