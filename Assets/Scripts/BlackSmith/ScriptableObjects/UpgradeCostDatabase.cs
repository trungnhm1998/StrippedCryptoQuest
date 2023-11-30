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
    }
}
