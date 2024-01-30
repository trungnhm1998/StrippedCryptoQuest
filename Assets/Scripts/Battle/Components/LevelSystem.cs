using System;
using CryptoQuest.Battle.Actions;
using CryptoQuest.Character.LevelSystem;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using IndiGames.Core.Events;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Components
{
    public class HeroLeveledUpAction : ActionBase
    {
        public HeroBehaviour Hero { get; }

        public HeroLeveledUpAction(HeroBehaviour hero)
        {
            Hero = hero;
        }
    }

    /// <summary>
    /// Hero only have exp, lvl will be calculated from exp at runtime
    /// </summary>
    public class LevelSystem : MonoBehaviour
    {
        public event Action ExpAdded;
        [SerializeField, ReadOnly] private int _level = 1;
        [SerializeField] private AttributeScriptableObject _expBuffAttribute;

        private IStatsConfigProvider _statsProvider;
        private IExpProvider _expProvider;
        private int _lastLevel = 1;
        public int LastLevel => _lastLevel;

        public int Level => CalculateCurrentLevel();

        /// I set this to true because If character already have exp      
        /// the hero level is not being updated and UI will not correct
        private bool _needToRecalculateLevel = true;
        private bool _isAddedExp = false;

        private HeroBehaviour _character;
        private AttributeSystemBehaviour _attributeSystem;

        private void Awake()
        {
            _character = GetComponent<HeroBehaviour>();
            _attributeSystem = GetComponent<AttributeSystemBehaviour>();
            _expProvider = GetComponent<IExpProvider>();
            _statsProvider = GetComponent<IStatsConfigProvider>();
        }

        public void AddExp(float expToAdd)
        {
            if (_character.IsValidAndAlive() == false)
            {
                Debug.LogWarning($"CharacterLevelComponent::AddExp: Failed because this character is dead");
                return;
            }

            _needToRecalculateLevel = true;
            _isAddedExp = true;

            _attributeSystem.TryGetAttributeValue(_expBuffAttribute, out var expBuffValue);

            if (expBuffValue.CurrentValue == 0f)
            {
                expBuffValue.CurrentValue = 1;
            }

            var addedExp = expToAdd * expBuffValue.CurrentValue;
            _expProvider.Exp += addedExp;
            ExpAdded?.Invoke();

            CalculateCurrentLevel();
        }

        private float _lastKnownExp;

        private int CalculateCurrentLevel()
        {
            if (_needToRecalculateLevel == false && Mathf.Approximately(_lastKnownExp, _expProvider.Exp)) return _level;
            _lastKnownExp = _expProvider.Exp;
            _needToRecalculateLevel = false;
            var levelCalculator = new LevelCalculator(_statsProvider.Stats.MaxLevel);
            _level = levelCalculator.CalculateCurrentLevel(_expProvider.Exp);
            if (_lastLevel < _level)
            {
                OnCharacterLevelUp();
            }

            _isAddedExp = false;
            _lastLevel = _level;

            return _level;
        }

        private void OnCharacterLevelUp()
        {
            ActionDispatcher.Dispatch(new HeroLeveledUpAction(_character));
            ActionDispatcher.Dispatch(new LevelUpAfterAddExpAction(_character, _isAddedExp));
        }

        private bool IsMaxedLevel => Level == _statsProvider.Stats.MaxLevel;

        public int GetNextLevelRequireExp()
        {
            var currentLevel = Level;
            return new LevelCalculator(_statsProvider.Stats.MaxLevel).RequiredExps[
                IsMaxedLevel ? currentLevel - 1 : currentLevel];
        }

        public int GetCurrentLevelExp()
        {
            var currentLevelAccumulateExp =
                new LevelCalculator(_statsProvider.Stats.MaxLevel).AccumulatedExps[Level - 1];
            return IsMaxedLevel
                ? GetNextLevelRequireExp()
                : (int)(_expProvider.Exp - currentLevelAccumulateExp);
        }
    }
}