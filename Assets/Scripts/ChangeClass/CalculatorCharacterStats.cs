using System.Collections;
using System.Collections.Generic;
using CryptoQuest.ChangeClass.View;
using CryptoQuest.Character.LevelSystem;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Helper;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace CryptoQuest.ChangeClass
{
    public class CalculatorCharacterStats : MonoBehaviour
    {
        private readonly ILevelAttributeCalculator _levelAttributeCalculator = new DefaultLevelAttributeCalculator();
        [SerializeField] private AttributeSystemBehaviour _baseAttributeSystem;
        [SerializeField] private UIPreviewClassMaterial _previewStats;
        private ILevelCalculator _calculator;
        private UICharacter _character;
        private int _maxLevel;
        private int _level;
        private bool _isFinishFetchData = false;

        public void CalculatorStats(UICharacter character)
        {
            _character = character;
            _maxLevel = _character.Class.Stats.MaxLevel;
            _calculator = new LevelCalculator(_maxLevel);
            SetAttributeBaseValue();
            CalculatorExp(character);
        }

        private void CalculatorExp(UICharacter character)
        {
            var exp = _character.Class.Experience;
            _level = _calculator.CalculateCurrentLevel(exp);
            var currentExp = GetCurrentExp(exp);
            var requireExp = GetRequiredExp(exp);
            character.CalculatorExp(currentExp, requireExp, _level);
        }

        private void SetAttributeBaseValue()
        {
            _isFinishFetchData = false;
            var attributeDefs = new List<CappedAttributeDef>(_character.Class.Stats.Attributes);

            foreach (var attributeDef in attributeDefs)
            {
                if (!_baseAttributeSystem.TryGetAttributeValue(attributeDef.Attribute, out var attributeValue)) continue;
                var baseValueAtLevel =
                    _levelAttributeCalculator.GetValueAtLevel(_level, attributeDef, _maxLevel);
                attributeValue.BaseValue = baseValueAtLevel;

                _baseAttributeSystem.SetAttributeValue(attributeDef.Attribute, attributeValue);
            }
            _isFinishFetchData = true;
            StartCoroutine(CalculatorNewAttributeValue());
        }

        private IEnumerator CalculatorNewAttributeValue()
        {
            yield return new WaitUntil(() => _isFinishFetchData);
            for (int i = 0; i < _baseAttributeSystem.AttributeValues.Count; i++)
            {
                var attributeValue = _baseAttributeSystem.AttributeValues[i];
                _baseAttributeSystem.AttributeValues[i] =
                    attributeValue.Attribute.CalculateInitialValue(attributeValue,
                        _baseAttributeSystem.AttributeValues);
            }
            _baseAttributeSystem.UpdateAttributeValues();
            _previewStats.PreviewCharacter(_baseAttributeSystem.AttributeValues, _character);
        }

        public float GetRequiredExp(float exp)
        {
            var level = _calculator.CalculateCurrentLevel(exp);
            var requireExp = _calculator.RequiredExps[level];
            return requireExp;
        }

        public float GetCurrentExp(float exp)
        {
            var level = _calculator.CalculateCurrentLevel(exp);
            var currentExp = exp - _calculator.AccumulatedExps[level - 1];
            return currentExp;
        }
    }
}