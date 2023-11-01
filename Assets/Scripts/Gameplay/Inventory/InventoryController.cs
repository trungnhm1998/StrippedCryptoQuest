using System.Collections;
using CryptoQuest.Core;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using CryptoQuest.System.SaveSystem.Actions;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    public interface IInventoryController
    {
        InventorySO Inventory { get; }

        bool Add(EquipmentInfo equipment);

        bool Remove(EquipmentInfo equipment);

        bool Add(ConsumableInfo consumable);

        bool Remove(ConsumableInfo consumable);
    }

    public class InventoryController : MonoBehaviour, IInventoryController
    {
        [SerializeField] private InventorySO _inventory;
        public InventorySO Inventory => _inventory;

        [Header("Listening to")]
        [SerializeField] private LootEventChannelSO _addLootRequestEventChannel;

        private IEquipmentDefProvider _definitionDatabase;

        private TinyMessenger.TinyMessageSubscriptionToken _listenToLoadCompletedEventToken;

        private void Awake()
        {
            ServiceProvider.Provide<IInventoryController>(this);
            _definitionDatabase = GetComponent<IEquipmentDefProvider>();
        }

        protected void OnEnable()
        {
            _addLootRequestEventChannel.EventRaised += AddLoot;
        }

        protected void OnDisable()
        {
            _addLootRequestEventChannel.EventRaised -= AddLoot;
        }

        private void Start()
        {
            _listenToLoadCompletedEventToken = ActionDispatcher.Bind<LoadInventoryCompletedAction>(_ => LoadInventory());            
            ActionDispatcher.Dispatch(new LoadInventoryAction(this));
        }

        private void LoadInventory()
        {
            ActionDispatcher.Unbind(_listenToLoadCompletedEventToken);
            StartCoroutine(LoadAllEquipment());
        }

        private IEnumerator LoadAllEquipment()
        {
            for (var index = 0; index < _inventory.Equipments.Count; index++)
            {
                var equipment = _inventory.Equipments[index];
                yield return _definitionDatabase.Load(equipment);
                _inventory.Equipments[index] = equipment;
            }
            _inventory.OnLoaded();
        }

        private void AddLoot(LootInfo loot)
        {
            loot.AddItemToInventory(_inventory);
            ActionDispatcher.Dispatch(new SaveInventoryAction(this));
        }

        public bool Add(EquipmentInfo equipment)
        {
            if (!_inventory.Add(equipment)) return false;
            ActionDispatcher.Dispatch(new SaveInventoryAction(this));
            return true;
        }

        public bool Remove(EquipmentInfo equipment)
        {
            if (!_inventory.Remove(equipment)) return false;
            ActionDispatcher.Dispatch(new SaveInventoryAction(this));
            return true;
        }

        public bool Add(ConsumableInfo consumable)
        {
            if (!_inventory.Add(consumable)) return false;
            ActionDispatcher.Dispatch(new SaveInventoryAction(this));
            return true;
        }

        public bool Remove(ConsumableInfo consumable)
        {
            var result = _inventory.Remove(consumable);
            ActionDispatcher.Dispatch(new SaveInventoryAction(this));
            return result;
        }
    }
}