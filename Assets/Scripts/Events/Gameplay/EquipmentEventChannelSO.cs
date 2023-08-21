using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEngine;
using UnityEngine.Events;
using ESlotType =
    CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquippingSlotContainer.EType;

namespace CryptoQuest.Events.Gameplay
{
    [CreateAssetMenu(menuName = "Core/Events/Equiment Event Channel", fileName = "EquipmentEventChannelSO")]
    public class EquipmentEventChannelSO : ScriptableObject
    {
        public UnityAction<ESlotType, EquipmentInfo> EventRaised;

#if UNITY_EDITOR
        [field: SerializeField] public ESlotType SlotType { get; private set; } = new();
        [field: SerializeField] public EquipmentInfo Equipment { get; private set; } = new();

#endif

        public void RaiseEvent(ESlotType type, EquipmentInfo equipment)
        {
            OnRaiseEvent(type, equipment);
        }

        private void OnRaiseEvent(ESlotType type, EquipmentInfo equipment = null)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

#if UNITY_EDITOR
            SlotType = type;
            Equipment = equipment;
#endif

            EventRaised.Invoke(type, equipment);
        }
    }
}