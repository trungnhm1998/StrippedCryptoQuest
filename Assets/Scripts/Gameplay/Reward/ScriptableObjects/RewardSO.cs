using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;
using UnityEngine;

namespace CryptoQuest.Gameplay.Reward.ScriptableObjects
{
    public class RewardSO : ScriptableObject
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
