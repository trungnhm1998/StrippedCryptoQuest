using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Loot;

namespace CryptoQuest.Character.Enemy
{
    /// <summary>
    /// TODO: Remove this class because <see cref="EnemyBehaviour"/> already handle all of this
    /// </summary>
    [Serializable]
    public class EnemySpec : CharacterInformation<EnemyDef, EnemySpec>
    {
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