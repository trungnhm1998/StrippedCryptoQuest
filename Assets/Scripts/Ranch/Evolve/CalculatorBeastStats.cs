using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Beast;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Helper;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Ranch.Evolve
{
    public class CalculatorBeastStats : MonoBehaviour
    {
        [SerializeField] private CalculatorBeastStatsSO calculatorBeastStatsSo;
        [SerializeField] private BeastAttributeSystemSO beastAttributeSystemSo;
        [SerializeField] private AttributeSystemBehaviour _attributeSystemBehaviour;
        private readonly ILevelAttributeCalculator _levelAttributeCalculator = new DefaultLevelAttributeCalculator();

        private void OnEnable()
        {
            calculatorBeastStatsSo.EventRaised += SetAttributeBaseValue;
        }

        private void OnDisable()
        {
            calculatorBeastStatsSo.EventRaised -= SetAttributeBaseValue;
        }

        private void SetAttributeBaseValue(IBeast beast)
        {
            var attributeDefs = new List<CappedAttributeDef>(beast.Stats.Attributes);

            foreach (var attributeDef in attributeDefs)
            {
                if (!_attributeSystemBehaviour.TryGetAttributeValue(attributeDef.Attribute, out var attributeValue))
                    continue;
                var baseValueAtLevel =
                    _levelAttributeCalculator.GetValueAtLevel(beast.Level, attributeDef, beast.MaxLevel);
                attributeValue.BaseValue = baseValueAtLevel;

                _attributeSystemBehaviour.SetAttributeValue(attributeDef.Attribute, attributeValue);
            }

            CalculatorNewAttributeValue();
        }

        private void CalculatorNewAttributeValue()
        {
            for (int i = 0; i < _attributeSystemBehaviour.AttributeValues.Count; i++)
            {
                var attributeValue = _attributeSystemBehaviour.AttributeValues[i];
                _attributeSystemBehaviour.AttributeValues[i] =
                    attributeValue.Attribute.CalculateInitialValue(attributeValue,
                        _attributeSystemBehaviour.AttributeValues);
            }

            _attributeSystemBehaviour.UpdateAttributeValues();
            beastAttributeSystemSo.RaiseEvent(_attributeSystemBehaviour);
        }
    }
}