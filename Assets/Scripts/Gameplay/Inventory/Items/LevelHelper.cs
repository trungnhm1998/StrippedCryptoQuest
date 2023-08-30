using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.Items
{
    public interface ILevelCalculator
    {
        float GetValueAtLevel(int level, CappedAttributeDef attributeDef, int maxLevel);
    }

    public class DefaultAttributeFromLevelCalculator : ILevelCalculator
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