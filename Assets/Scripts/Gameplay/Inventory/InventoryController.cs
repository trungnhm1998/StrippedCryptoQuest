using System;
using System.Collections;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public interface IInventoryController
    {
        public event Action EquipmentsLoaded;
        void Add(EquipmentInfo equipment);
        void Remove(EquipmentInfo equipment);
        InventorySO Inventory { get; }
        bool Remove(ConsumableInfo consumable);
    }

    public class InventoryController : MonoBehaviour, IInventoryController
    {
        public event Action EquipmentsLoaded;
        [SerializeField] private EquipmentDatabaseSO _equipmentDatabase;
        [SerializeField] private InventorySO _inventory;
        public InventorySO Inventory => _inventory;

        [Header("Listening to")]
        [SerializeField] private LootEventChannelSO _addLootRequestEventChannel;

        private IEquipmentDefProvider _definitionDatabase;

        private void Awake()
        {
            ServiceProvider.Provide<IInventoryController>(this);
            _definitionDatabase = GetComponent<IEquipmentDefProvider>();
            StartCoroutine(LoadAllEquipment());
        }

        private IEnumerator LoadAllEquipment()
        {
            foreach (var equipment in _inventory.Equipments)
            {
                var defId = equipment.DefinitionId;
                yield return _definitionDatabase.CoLoadEquipmentById(defId);
                var def = _definitionDatabase.GetEquipmentDefById(defId);
                equipment.Def = def;
            }

            EquipmentsLoaded?.Invoke();
        }

        private void OnEnable()
        {
            _addLootRequestEventChannel.EventRaised += AddLoot;
        }

        private void OnDisable()
        {
            _addLootRequestEventChannel.EventRaised -= AddLoot;
        }

        private void AddLoot(LootInfo loot) => loot.AddItemToInventory(_inventory);
        public void Add(EquipmentInfo equipment) => _inventory.Add(equipment);
        public void Remove(EquipmentInfo equipment) => _inventory.Remove(equipment);
        public bool Remove(ConsumableInfo consumable) => _inventory.Remove(consumable);
    }
}