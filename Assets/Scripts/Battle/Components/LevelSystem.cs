using System;
using CryptoQuest.AbilitySystem;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Character.LevelSystem;
using CryptoQuest.Gameplay.Helper;
using UnityEngine;
using AttributeScriptableObject = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.Battle.Components
{
    /// <summary>
    /// Hero only have exp, lvl will be calculated from exp at runtime
    /// </summary>
    [RequireComponent(typeof(HeroBehaviour))]
    public class LevelSystem : MonoBehaviour
    {
        public static event Action<HeroBehaviour> HeroLeveledUp;

        [SerializeField] private AttributeScriptableObject _expBuffAttribute;
        [SerializeField] private AttributeWithMaxCapped[] _levelUpResetAttributes;

        // I use static here so we only have to create and init level calculator once
        public static ILevelCalculator LevelCalculator { get; private set; }
        private ILevelAttributeCalculator _levelAttributeCalculator = new DefaultLevelAttributeCalculator();
        private int _lastLevel = 0;
        private HeroBehaviour _character;

        public int Level
        {
            get
            {
                var currentLevel = LevelCalculator.CalculateCurrentLevel(_character.Spec.Experience);
                return currentLevel <= 0 ? 1 : currentLevel;
            }
        }

        public void Init(HeroBehaviour character)
        {
            _character = character;
            LevelCalculator ??= new LevelCalculator(_character.Stats.MaxLevel);
            _lastLevel = _character.Level;
            CalculateLevel();
        }

        public void AddExp(float expToAdd)
        {
            if (_character.HasTag(TagsDef.Dead))
            {
                Debug.LogWarning($"CharacterLevelComponent::AddExp: Failed because this character is dead");
                return;
            }

            var attributeSystem = _character.AttributeSystem;
            attributeSystem.TryGetAttributeValue(_expBuffAttribute, out var expBuffValue);

            if (expBuffValue.CurrentValue == 0f)
            {
                expBuffValue.CurrentValue = 1;
            }

            var addedExp = expToAdd * expBuffValue.CurrentValue;

            _character.RequestAddExp(addedExp);

            CalculateLevel();
        }

        private void CalculateLevel()
        {
            _character.Level = LevelCalculator.CalculateCurrentLevel(_character.Spec.Experience);

            if (_lastLevel < _character.Level)
            {
                CharacterLevelUp();
            }

            _lastLevel = _character.Level;
        }

        private void CharacterLevelUp()
        {
            RecalculateStats();
            HeroLeveledUp?.Invoke(_character);
        }

        /// <summary>
        /// Calculate stats of last level then calculate stats of current level then 
        /// add the addition value to base
        /// </summary>
        private void RecalculateStats()
        {
            var attributeSystem = _character.AttributeSystem;
            var attributeDefs = _character.Stats.Attributes;
            var characterAllowedMaxLvl = _character.Stats.MaxLevel;

            for (var i = 0; i < attributeDefs.Length; i++)
            {
                var attributeDef = attributeDefs[i];
                if (!attributeSystem.TryGetAttributeValue(attributeDef.Attribute, out var attributeValue)) continue;

                var lastLevelBaseValue =
                    _levelAttributeCalculator.GetValueAtLevel(_lastLevel, attributeDef, characterAllowedMaxLvl);
                var currentLevelBaseValue =
                    _levelAttributeCalculator.GetValueAtLevel(_character.Level, attributeDef,
                        characterAllowedMaxLvl);
                var additionBaseValue = currentLevelBaseValue - lastLevelBaseValue;

                attributeSystem.SetAttributeBaseValue(attributeDef.Attribute,
                    attributeValue.BaseValue + additionBaseValue);
            }

            ReInitCappedAttributes();
            attributeSystem.UpdateAttributeValues();
        }

        private void ReInitCappedAttributes()
        {
            var attributeSystem = _character.AttributeSystem;
            for (int i = 0; i < _levelUpResetAttributes.Length; i++)
            {
                var attributeToReset = _levelUpResetAttributes[i];
                if (!attributeSystem.TryGetAttributeValue(attributeToReset, out var attributeValue)) continue;
                var resetValue =
                    attributeToReset.CalculateInitialValue(attributeValue, attributeSystem.AttributeValues);
                attributeSystem.SetAttributeValue(attributeToReset, resetValue);
            }
        }

        private bool IsMaxedLevel => _character.Level == _character.Stats.MaxLevel;

        public int GetNextLevelRequireExp()
        {
            var currentLevel = _character.Level;
            return LevelCalculator.RequiredExps[IsMaxedLevel ? currentLevel - 1 : currentLevel];
        }

        public int GetCurrentLevelExp()
        {
            var currentLevelAccumulateExp = LevelCalculator.AccumulatedExps[_character.Level - 1];
            return IsMaxedLevel
                ? GetNextLevelRequireExp()
                : (int)(_character.Spec.Experience - currentLevelAccumulateExp);
        }
    }
}