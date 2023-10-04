using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class EvolvePresenter : MonoBehaviour
    {
        [SerializeField] private UnityEvent<InventorySO> _getInventoryEvent;

        private void Start()
        {
            GetInventory();
        }

        private void GetInventory()
        {
            var inventory = ServiceProvider.GetService<IInventoryController>().Inventory;
            _getInventoryEvent.Invoke(inventory);
        }

    }
}