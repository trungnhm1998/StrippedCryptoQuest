using UnityEngine;
using CryptoQuest.Events;
using AttributeScriptableObject = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay.Helper;

namespace CryptoQuest.Gameplay.Character.LevelSystem
{
    public class CharacterLevelComponent : MonoBehaviour, ICharacterComponent
    {
        [SerializeField] private CharacterSpecEventChannelSO _characterLevelUpEventChannel;
        [SerializeField] private AttributeScriptableObject _expBuffAttribute;
        [SerializeField] private AttributeWithMaxCapped[] _levelUpResetAttributes;

        // I use static here so we only have to create and init level calculator once
        public static ILevelCalculator LevelCalculator { get; private set; }
        private ILevelAttributeCalculator _levelAttributeCalculator = new DefaultLevelAttributeCalculator();
        private int _lastLevel = 0;
        private CharacterBehaviourBase _character;
        private CharacterSpec _characterSpec;

        public void Init(CharacterBehaviourBase character)
        {
            _character = character;
            _characterSpec = character.Spec;
            LevelCalculator ??= new LevelCalculator(_characterSpec.StatsDef.MaxLevel);
            CalculateLevel();
        }

        public void AddExp(float expToAdd)
        {
            if (_character.IsDead())
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

            _characterSpec.Experience += addedExp;

            CalculateLevel();
        }

        private void CalculateLevel()
        {
            _characterSpec.Level = LevelCalculator.CalculateCurrentLevel(_characterSpec.Experience);

            if (_lastLevel < _characterSpec.Level)
            {
                CharacterLevelUp();
            }
            _lastLevel = _characterSpec.Level;
        }
        
        private void CharacterLevelUp()
        {
            RecalculateStats();
            _characterLevelUpEventChannel.RaiseEvent(_characterSpec);
        }

        /// <summary>
        /// Calculate stats of last level then calculate stats of current level then 
        /// add the addition value to base
        /// </summary>
        private void RecalculateStats()
        {
            var attributeSystem = _character.AttributeSystem;
            var attributeDefs = _characterSpec.StatsDef.Attributes;
            var characterAllowedMaxLvl = _characterSpec.StatsDef.MaxLevel;

            for (var i = 0; i < attributeDefs.Length; i++)
            {
                var attributeDef = attributeDefs[i];
                if (!attributeSystem.TryGetAttributeValue(attributeDef.Attribute, out var attributeValue)) continue;

                var lastLevelBaseValue =
                    _levelAttributeCalculator.GetValueAtLevel(_lastLevel, attributeDef, characterAllowedMaxLvl);
                var currentLevelBaseValue = 
                    _levelAttributeCalculator.GetValueAtLevel(_characterSpec.Level, attributeDef, characterAllowedMaxLvl);
                var additionBaseValue = currentLevelBaseValue - lastLevelBaseValue;

                attributeSystem.SetAttributeBaseValue(attributeDef.Attribute, attributeValue.BaseValue + additionBaseValue);
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
                var resetValue = attributeToReset.CalculateInitialValue(attributeValue, attributeSystem.AttributeValues);
                attributeSystem.SetAttributeValue(attributeToReset, resetValue);
            }
        }
    }
}
