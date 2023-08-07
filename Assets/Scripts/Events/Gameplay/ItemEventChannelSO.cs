using CryptoQuest.Gameplay.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.Gameplay
{
    [CreateAssetMenu(fileName = "ItemEventChannelSO", menuName = "Core/Events/Item Event Channel")]
    public class ItemEventChannelSO : ScriptableObject
    {
        public UnityAction<UsableInfo> EventRaised;

        public void RaiseEvent(UsableInfo itemBaseItem)
        {
            OnRaiseEvent(itemBaseItem);
        }

        private void OnRaiseEvent(UsableInfo itemBaseItem)
        {
            this.CallEventSafely(EventRaised, itemBaseItem);
        }
    }
}