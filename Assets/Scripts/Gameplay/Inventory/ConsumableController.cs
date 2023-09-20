using System;
using CryptoQuest.Gameplay.Inventory.Items;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public class ConsumableController : MonoBehaviour
    {
        [Header("Listening to")]
        [SerializeField] private ConsumableEventChannel _consumingItemEvent;

        [Header("Raise on")]
        [SerializeField] private ConsumableEventChannel _itemConsumedEvent;

        private IInventoryController _inventoryController;

        private void Awake()
        {
            _inventoryController = GetComponent<IInventoryController>();
        }

        private void OnEnable()
        {
            _consumingItemEvent.EventRaised += ConsumeItem;
        }

        private void OnDisable()
        {
            _consumingItemEvent.EventRaised -= ConsumeItem;
        }

        private void ConsumeItem(ConsumableInfo consumable)
        {
            _inventoryController.Remove(consumable);
            _itemConsumedEvent.RaiseEvent(consumable);
        }
    }
}