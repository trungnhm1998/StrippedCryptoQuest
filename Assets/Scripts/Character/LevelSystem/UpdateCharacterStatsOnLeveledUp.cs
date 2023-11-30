using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Helper;
using CryptoQuest.Sagas;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Character.LevelSystem
{
    public class UpdateCharacterStatsOnLeveledUp : SagaBase<HeroLeveledUpAction>
    {
        [SerializeField] private AttributeWithMaxCapped[] _attributesToResetWhenLeveledUp;
        private readonly ILevelAttributeCalculator _levelAttributeCalculator = new DefaultLevelAttributeCalculator();

        protected override void HandleAction(HeroLeveledUpAction ctx) => RecalculateStats(ctx.Hero);

        /// <summary>
        /// 1. Calculate stats of last level
        /// 2. calculate stats of current level
        /// 3. add the addition value to base
        /// </summary>
        /// <param name="hero"></param>
        private void RecalculateStats(HeroBehaviour hero)
        {
            if (!hero.TryGetComponent(out IStatsConfigProvider statsConfigProvider)) return;
            if (!hero.TryGetComponent(out Battle.Components.LevelSystem levelSystem)) return;
            var lastLevel = levelSystem.LastLevel;
            var level = levelSystem.Level;
            var attributeSystem = hero.AttributeSystem;
            if (attributeSystem == null) return;
            var stats = statsConfigProvider.Stats;
            var attributeDefs = stats.Attributes;
            var allowedMaxLvl = stats.MaxLevel;

            foreach (var attributeDef in attributeDefs)
            {
                if (!attributeSystem.TryGetAttributeValue(attributeDef.Attribute, out var attributeValue)) continue;

                var lastLevelBaseValue =
                    _levelAttributeCalculator.GetValueAtLevel(lastLevel, attributeDef, allowedMaxLvl);
                var currentLevelBaseValue =
                    _levelAttributeCalculator.GetValueAtLevel(level, attributeDef, allowedMaxLvl);
                var additionBaseValue = currentLevelBaseValue - lastLevelBaseValue;

                attributeSystem.SetAttributeBaseValue(attributeDef.Attribute,
                    attributeValue.BaseValue + additionBaseValue);
            }

            foreach (var attributeToReset in _attributesToResetWhenLeveledUp)
            {
                if (!attributeSystem.TryGetAttributeValue(attributeToReset, out var attributeValue)) continue;
                var resetValue =
                    attributeToReset.CalculateInitialValue(attributeValue, attributeSystem.AttributeValues);
                attributeSystem.SetAttributeValue(attributeToReset, resetValue);
            }

            attributeSystem.UpdateAttributeValues();
        }
    }
}