using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Events;
using CryptoQuest.System;
using UnityEngine;
using System.Collections;
using System;

namespace CryptoQuest.Gameplay.Inventory
{
    public interface IInventoryController
    {
        void Add(EquipmentInfo equipment);
        void Add(EquipmentInfo equipment, string equipmentId);
        void Remove(EquipmentInfo equipment);
        InventorySO Inventory { get; }
    }

    public class InventoryController : MonoBehaviour, IInventoryController
    {
        public event Action<EquipmentInfo> EquipmentLoadedEvent;
        [SerializeField] private EquipmentDatabaseSO _equipmentDatabase;
        [SerializeField] private InventorySO _inventory;
        [SerializeField] private ServiceProvider _provider;
        public InventorySO Inventory => _inventory;

        [Header("Listening to")]
        [SerializeField] private LootEventChannelSO _addLootRequestEventChannel;

        private void Awake()
        {
            _provider.Provide(this);
        }

        private void OnEnable()
        {
            _addLootRequestEventChannel.EventRaised += AddLoot;
        }

        private void OnDisable()
        {
            _addLootRequestEventChannel.EventRaised -= AddLoot;
        }

        private void AddLoot(LootInfo loot)
        {
            loot.AddItemToInventory(_inventory);
        }

        public void Add(EquipmentInfo equipment)
        {
            _inventory.Add(equipment);
        }

        public void Remove(EquipmentInfo equipment)
        {
            _inventory.Remove(equipment);
        }

        /// <summary>
        /// When pass data Id controller will load data using that id
        /// You can also use <see cref="AddAsync"/>
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="equipmentId"></param>
        public void Add(EquipmentInfo equipment, string equipmentId)        
        {
            StartCoroutine(AddAsync(equipment, equipmentId));
        }
        
        private IEnumerator AddAsync(EquipmentInfo equipment, string equipmentId)
        {
            yield return _equipmentDatabase.LoadDataById(equipmentId);
            var equipmentData = _equipmentDatabase.GetDataById(equipmentId);
            equipment.Data = equipmentData;
            Add(equipment);
            EquipmentLoadedEvent?.Invoke(equipment);
        }
    }
}