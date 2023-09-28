using System;
using CryptoQuest.Gameplay.Loot;

namespace CryptoQuest.Battle
{
    [Serializable]
    public struct CompletedContext
    {
        public LootInfo[] Loots;
    }
}