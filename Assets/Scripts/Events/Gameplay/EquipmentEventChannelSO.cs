using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events.Gameplay
{
    [CreateAssetMenu(menuName = "Core/Events/Equiment Event Channel", fileName = "EquipmentEventChannelSO")]
    public class EquipmentEventChannelSO : ScriptableObject
    {
        public UnityAction<EquippingSlotContainer.EType, EquipmentInfo> EventRaised;

        public void RaiseEvent(EquippingSlotContainer.EType type, EquipmentInfo equipment)
        {
            OnRaiseEvent(type, equipment);
        }

        private void OnRaiseEvent(EquippingSlotContainer.EType type, EquipmentInfo equipment = null)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EventRaised.Invoke(type, equipment);
        }
    }
}