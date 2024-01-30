using UnityEngine;

namespace CryptoQuest.Gameplay.Helper
{
    public interface ILevelAttributeCalculator
    {
        float GetValueAtLevel(int level, CappedAttributeDef attributeDef, int maxLevel);
    }

    public class DefaultLevelAttributeCalculator : ILevelAttributeCalculator
    {
        public float GetValueAtLevel(int level, CappedAttributeDef attributeDef, int maxLevel)
        {
            var currentLvl = Mathf.Clamp(level, 1, maxLevel);
            var value = Mathf.Floor((attributeDef.MaxValue - attributeDef.MinValue) / maxLevel * currentLvl) +
                        attributeDef.MinValue;

            value += attributeDef.ModifyValue + attributeDef.RandomValue;

            return value;
        }
    }
}