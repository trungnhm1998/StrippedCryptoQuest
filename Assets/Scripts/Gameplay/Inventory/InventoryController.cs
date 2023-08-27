using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public interface IInventoryController
    {
        void Add(EquipmentInfo equipment);
    }

    public class InventoryController : MonoBehaviour, IInventoryController
    {
        [SerializeField] private ServiceProvider _provider;

        private void Awake()
        {
            _provider.Provide(this);
        }

        public void Add(EquipmentInfo equipment)
        {
            _provider.Inventory.Add(equipment);
        }
    }
}