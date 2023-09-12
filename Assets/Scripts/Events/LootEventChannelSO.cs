using CryptoQuest.Gameplay.Loot;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events
{
    [CreateAssetMenu(menuName = "Crypto Quest/Events/Loot Event Channel", fileName = "LootEventChannel")]
    public class LootEventChannelSO : ScriptableObject
    {
        public UnityAction<LootInfo> EventRaised;

        public void RaiseEvent(LootInfo loot)
        {
            this.CallEventSafely<LootInfo>(EventRaised, loot);
        }
    }
}