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
            var value = attributeDef.MinValue;
            var currentLvl = Mathf.Clamp(level, 1, maxLevel);
            value = Mathf.Floor((attributeDef.MaxValue - attributeDef.MinValue) / maxLevel * currentLvl) +
                    attributeDef.MinValue;

            return value;
        }
    }
}