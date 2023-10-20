using System;
using CryptoQuest.Gameplay.Loot;
using IndiGames.Core.SaveSystem.ScriptableObjects;

namespace CryptoQuest.Gameplay.Reward.ScriptableObjects
{
    public class RewardSO : SerializableScriptableObject
    {
        public event Action<LootInfo[]> OnRewardEvent;
        public event Action<float> OnRewardExpEvent;

        public void RewardRaiseEvent(LootInfo[] loots)
        {
            OnRewardEvent?.Invoke(loots);
        }

        public void RewardExpEvent(float exp)
        {
            OnRewardExpEvent?.Invoke(exp);
        }
    }
}
