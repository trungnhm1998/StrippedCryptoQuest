using CryptoQuest.Gameplay.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.Gameplay
{
    [CreateAssetMenu(fileName = "ItemEventChannelSO", menuName = "Core/Events/Item Event Channel")]
    public class ItemEventChannelSO : ScriptableObject
    {
        public UnityAction<ItemBase> EventRaised;

        public void RaiseEvent(ItemBase itemBaseItem)
        {
            OnRaiseEvent(itemBaseItem);
        }

        private void OnRaiseEvent(ItemBase itemBaseItem)
        {
            this.CallEventSafely(EventRaised, itemBaseItem);
        }
    }
}