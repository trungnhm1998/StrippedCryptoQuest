using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Loot;
using UnityEngine;

namespace CryptoQuest.Character.Enemy
{
    /// <summary>
    /// TODO: Remove this class because <see cref="EnemyBehaviour"/> already handle all of this
    /// </summary>
    [Serializable]
    public class EnemySpec
    {
        [field: SerializeField] public EnemyDef Data { get; protected set; }
        public virtual void Init(EnemyDef data) => Data = data;
        public virtual bool IsValid() => Data != null;

        public virtual void Release()
        {
            Data = null; // remove ref count from the SO which we dynamically loaded
        }

        /// <summary>
        /// Get all lootable items from enemy based on their <see cref="Drop"/> configs
        /// </summary>
        /// <returns>Cloned loot</returns>
        public List<LootInfo> GetLoots()
        {
            var drops = Data.Drops;
            var loots = new List<LootInfo>();
            foreach (var drop in drops) loots.Add(drop.CreateLoot());
            return loots;
        }
    }
}