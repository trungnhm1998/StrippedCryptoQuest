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
        InventorySO Inventory { get; }

        void Add(EquipmentInfo equipment);

        void Remove(EquipmentInfo equipment);

        bool Remove(ConsumableInfo consumable);
    }

    public class InventoryController : MonoBehaviour, IInventoryController, ISaveObject
    {
        [SerializeField] private InventorySO _inventory;
        public InventorySO Inventory => _inventory;

        [Header("Listening to")]
        [SerializeField] private LootEventChannelSO _addLootRequestEventChannel;

        private IEquipmentDefProvider _definitionDatabase;
        private ISaveSystem _saveSystem;

        private void Awake()
        {
            ServiceProvider.Provide<IInventoryController>(this);
            _definitionDatabase = GetComponent<IEquipmentDefProvider>();
            _saveSystem = ServiceProvider.GetService<ISaveSystem>();
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
            // _saveSystem?.SaveObject(this);
        }

        private void OnEnable()
        {
            _addLootRequestEventChannel.EventRaised += AddLoot;
        }

        private void OnDisable()
        {
            _addLootRequestEventChannel.EventRaised -= AddLoot;
        }

        private void Start()
        {
            // Equipement should be loaded after load save data
            // Otherwise just load it from SO as is
            if(_saveSystem == null || !_saveSystem.LoadObject(this))
            {
                StartCoroutine(LoadAllEquipment());
            }
        }

        private void AddLoot(LootInfo loot) => loot.AddItemToInventory(_inventory);

        public void Add(EquipmentInfo equipment)
        {
            _inventory.Add(equipment);
            _saveSystem?.SaveObject(this);
        }

        public void Remove(EquipmentInfo equipment)
        {
            _inventory.Remove(equipment);
            _saveSystem?.SaveObject(this);
        }

        public bool Remove(ConsumableInfo consumable)
        {
            var result = _inventory.Remove(consumable);
            _saveSystem?.SaveObject(this);
            return result;
        }

        #region SaveSystem

        public string Key
        {
            get { return "Inventory"; }
        }

        public string ToJson()
        {
            return _inventory.ToJson();
        }

        public bool FromJson(string json)
        {
            StartCoroutine(CoFromJson(json));
            return true;
        }

        public IEnumerator CoFromJson(string json)
        {
            yield return StartCoroutine(_inventory.CoFromJson(json));
            yield return StartCoroutine(LoadAllEquipment());
        }
        #endregion
    }
}