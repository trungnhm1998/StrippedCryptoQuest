using System;
using UnityEngine;
using CryptoQuest.Events;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;

namespace CryptoQuest.Gameplay.Character.LevelSystem
{
    public class LevelController : MonoBehaviour
    {
        public static Action<CharacterSpec, float> AddExpRequested;

        [SerializeField] private AttributeScriptableObject _expBuffAttribute;
        [SerializeField] private CharacterSpecEventChannelSO _characterLevelUpEventChannel;

        private void OnEnable()
        {
            AddExpRequested += AddExpToCharacter;
        }

        private void OnDisable()
        {
            AddExpRequested -= AddExpToCharacter;
        }

        private void AddExpToCharacter(CharacterSpec character, float expToAdd)
        {
            var attributeSystem = character.CharacterComponent.AttributeSystem;
            attributeSystem.TryGetAttributeValue(_expBuffAttribute, out var _expBuffValue);

            if (_expBuffValue.CurrentValue == 0f)
            {
                _expBuffValue.CurrentValue = 1;
            }

            var lastLevel = character.Level;
            var addedExp = expToAdd * _expBuffValue.CurrentValue;

            character.Experience += addedExp;

            if (lastLevel < character.Level)
            {
                _characterLevelUpEventChannel.RaiseEvent(character);
            }
        }
    }
}
