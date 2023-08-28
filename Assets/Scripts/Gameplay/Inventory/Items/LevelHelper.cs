using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.Items
{
    public interface ILevelCalculator
    {
        float GetValueAtLevel(int level, CappedAttributeDef attributeDef, StatsDef stats);
    }

    public class DefaultAttributeFromLevelCalculator : ILevelCalculator
    {
        public float GetValueAtLevel(int level, CappedAttributeDef attributeDef, StatsDef stats)
        {
            var value = attributeDef.MinValue;
            var currentLvl = Mathf.Clamp(level, 1, stats.MaxLevel);
            value = Mathf.Floor((attributeDef.MaxValue - attributeDef.MinValue) / stats.MaxLevel * currentLvl) +
                    attributeDef.MinValue;

            return value;
        }
    }
}