using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public interface IInventoryController
    {
        public void AddItem(EquipmentInfo item);
    }

    public class InventoryController : MonoBehaviour, IInventoryController
    {
        [SerializeField] private ServiceProvider _provider;

        private IInventory _inventory;

        private void Awake()
        {
            _provider.Provide(this);
        }

        public void AddItem(EquipmentInfo item) { }
    }
}