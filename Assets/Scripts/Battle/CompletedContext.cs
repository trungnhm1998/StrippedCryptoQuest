using System;
using CryptoQuest.Gameplay.Loot;

namespace CryptoQuest.Battle
{
    [Obsolete]
    [Serializable]
    public struct CompletedContext
    {
        public LootInfo[] Loots;
    }
}