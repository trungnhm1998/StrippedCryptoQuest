using System;
using System.Collections;
using CryptoQuest.Gameplay.Battle.Core;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.ModifierComputationStrategies;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CryptoQuest.Gameplay.Inventory.Equipment
{
    public class CharacterEquipmentsBehaviour : MonoBehaviour
    {
        [Serializable]
        public struct Equipment
        {
            public EquipmentSO Data;
            public Stats Stats => Data.Stats;
            public int Level;
        }

        [SerializeField] private EffectSystemBehaviour _effectSystemBehaviour;
        [SerializeField] private Equipment _equipment;
        [SerializeField] private InfiniteEffectScriptableObject _baseEquipmentEffect;
        [SerializeField] private ConstantFloatComputationSO _computationMethod;
        private InfiniteEffectScriptableObject _equipmentEffect;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            Equip();
        }

        public void Equip()
        {
            // get all attributes in equipment stats
            var attributes = _equipment.Stats.Attributes;
            _equipmentEffect = Instantiate(_baseEquipmentEffect);
            var modifiers = new EffectAttributeModifier[attributes.Length];
            for (int i = 0; i < attributes.Length; i++)
            {
                var attribute = attributes[i];
                modifiers[i] = new EffectAttributeModifier
                {
                    AttributeSO = attribute.AttributeDef,
                    ModifierType = EAttributeModifierType.Add,
                    ModifierComputationMethod = _computationMethod,
                    Value = attribute.MinValue
                };
            }

            _equipmentEffect.EffectDetails.Modifiers = modifiers;

            var effect = _effectSystemBehaviour.GetEffect(_equipmentEffect, this, new SkillParameters());
            _effectSystemBehaviour.ApplyEffectToSelf(effect);
        }

        public void UnEquip() { }
    }
}