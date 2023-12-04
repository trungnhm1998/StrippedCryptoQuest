using System;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.BlackSmith.ScriptableObjects
{
    public class UpgradeCostDatabase : ScriptableObject
    {
        [field: SerializeField] public CostByRarity[] CostData { get;
        #if UNITY_EDITOR 
            set; 
        #endif
        }
    }

    [Serializable]
    public struct CostByRarity
    {
        public int RarityID;

        [Tooltip("Index i is cost from level i+1 to level i+2")]
        public int[] Costs;

        public float GetCost(int currentLevel, int toLevel)
        {
            // Index is imported base on master data in this format
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
