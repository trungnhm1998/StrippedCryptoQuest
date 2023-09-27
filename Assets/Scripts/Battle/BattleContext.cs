using System;
using CryptoQuest.Gameplay.Loot;

namespace CryptoQuest.Battle
{
    [Serializable]
    public struct BattleContext
    {
        public LootInfo[] Loots;
    }
}