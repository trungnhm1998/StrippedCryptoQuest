using System;
using CryptoQuest.Gameplay.Loot;
using UnityEngine;

namespace CryptoQuest.Character.Enemy
{
    [Serializable]
    public class Drop
    {
        [field: SerializeField]
        public bool Stealable { get; private set; }

        [field: SerializeField]
        public float Chance { get; private set; } = 1f;

        [field: SerializeReference, SubclassSelector]
        public LootInfo Loot { get; private set; }

        /// <returns>a cloned of loot config</returns>
        public LootInfo GetLoot() => Loot.Clone();
    }
}