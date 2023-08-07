using CryptoQuest.Gameplay.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.Gameplay
{
    [CreateAssetMenu(fileName = "ItemEventChannelSO", menuName = "Core/Events/Item Event Channel")]
    public class ItemEventChannelSO : ScriptableObject
    {
        public UnityAction<UsableInformation> EventRaised;

        public void RaiseEvent(UsableInformation itemBaseItem)
        {
            OnRaiseEvent(itemBaseItem);
        }

        private void OnRaiseEvent(UsableInformation itemBaseItem)
        {
            this.CallEventSafely(EventRaised, itemBaseItem);
        }
    }
}