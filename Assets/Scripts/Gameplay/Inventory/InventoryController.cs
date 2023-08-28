using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public interface IInventoryController
    {
        void Add(EquipmentInfo equipment);
        void Remove(EquipmentInfo equipment);
        InventorySO Inventory { get; }
    }

    public class InventoryController : MonoBehaviour, IInventoryController
    {
        [SerializeField] private InventorySO _inventory;
        [SerializeField] private ServiceProvider _provider;
        public InventorySO Inventory => _inventory;

        private void Awake()
        {
            _provider.Provide(this);
        }

        public void Add(EquipmentInfo equipment)
        {
            _inventory.Add(equipment);
        }

        public void Remove(EquipmentInfo equipment)
        {
            _inventory.Remove(equipment);
        }
    }
}