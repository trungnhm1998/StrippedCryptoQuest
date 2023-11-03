using CryptoQuest.Character.LevelSystem;
using CryptoQuest.Core;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
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
    [RequireComponent(typeof(HeroBehaviour))]
    public class LevelSystem : MonoBehaviour
    {
        [SerializeField, ReadOnly] private int _level = 1;
        [SerializeField] private AttributeScriptableObject _expBuffAttribute;

        private ILevelCalculator _levelCalculator;

        // TODO: Possible optimization
        private ILevelCalculator LevelCalculator => _levelCalculator ??= new LevelCalculator(Hero.Stats.MaxLevel);
        private HeroBehaviour _character;
        private HeroBehaviour Hero => _character ??= GetComponent<HeroBehaviour>();
        private IExpProvider _expProvider;
        private IExpProvider ExpProvider => _expProvider ??= GetComponent<IExpProvider>();
        private int _lastLevel;
        public int LastLevel => _lastLevel;

        public int Level => CalculateCurrentLevel();

        public void AddExp(float expToAdd)
        {
            if (Hero.IsValidAndAlive() == false)
            {
                Debug.LogWarning($"CharacterLevelComponent::AddExp: Failed because this character is dead");
                return;
            }

            var attributeSystem = Hero.AttributeSystem;
            attributeSystem.TryGetAttributeValue(_expBuffAttribute, out var expBuffValue);

            if (expBuffValue.CurrentValue == 0f)
            {
                expBuffValue.CurrentValue = 1;
            }

            var addedExp = expToAdd * expBuffValue.CurrentValue;

            Hero.RequestAddExp(addedExp);

            CalculateCurrentLevel();
        }

        private int CalculateCurrentLevel()
        {
            _level = LevelCalculator.CalculateCurrentLevel(ExpProvider.Exp);
            _level = Mathf.Max(1, _level);
            if (_lastLevel < _level) OnCharacterLevelUp();
            _lastLevel = _level;
            return _level;
        }

        private void OnCharacterLevelUp() => ActionDispatcher.Dispatch(new HeroLeveledUpAction(_character));
        private bool IsMaxedLevel => Level == Hero.Stats.MaxLevel;

        public int GetNextLevelRequireExp()
        {
            var currentLevel = Level;
            return LevelCalculator.RequiredExps[IsMaxedLevel ? currentLevel - 1 : currentLevel];
        }

        public int GetCurrentLevelExp()
        {
            var currentLevelAccumulateExp = LevelCalculator.AccumulatedExps[Level - 1];
            return IsMaxedLevel
                ? GetNextLevelRequireExp()
                : (int)(ExpProvider.Exp - currentLevelAccumulateExp);
        }
    }
}