using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;
using IndiGames.Core.Events.ScriptableObjects;

namespace CryptoQuest.Gameplay.Reward.Events
{
    public class RewardLootEvent : GenericEventChannelSO<List<LootInfo>> { }
}