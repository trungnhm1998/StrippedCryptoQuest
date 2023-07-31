using CryptoQuest.Gameplay.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.Gameplay
{
    [CreateAssetMenu(fileName = "ItemEventChannelSO", menuName = "Core/Events/Item Event Channel")]
    public class ItemEventChannelSO : ScriptableObject
    {
        public UnityAction<ItemInfomation> EventRaised;

        public void RaiseEvent(ItemInfomation item)
        {
            OnRaiseEvent(item);
        }

        private void OnRaiseEvent(ItemInfomation item)
        {
            this.CallEventSafely(EventRaised, item);
        }
    }
}