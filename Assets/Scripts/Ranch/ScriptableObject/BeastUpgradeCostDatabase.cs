using System;
using UnityEngine;

namespace CryptoQuest.Ranch.ScriptableObject
{
    public class BeastUpgradeCostDatabase : UnityEngine.ScriptableObject
    {
        [field: SerializeField] public BeastCost CostData { get; private set; }

#if UNITY_EDITOR
        public void Editor_SetCostData(BeastCost data)
        {
            CostData = data;
        }
#endif
    }

    [Serializable]
    public class BeastCost
    {
        [Tooltip("Index i is cost from level i+1 to level i+2")]
        public int[] Costs;

        public float GetCost(int currentLevel, int toLevel)
        {
            var currentIndex = currentLevel - 1;
            var toIndex = toLevel - 1;
            int totalCost = 0;

            for (int i = currentIndex; i < toIndex; i++)
            {
                totalCost += Costs[i];
            }

            return totalCost;
        }
    }
}