using System.Collections;
using CryptoQuest.Events;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using CryptoQuest.System.SaveSystem;
using IndiGames.Core.SaveSystem;
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

    public class InventoryController : MonoBehaviour, IInventoryController, IJsonSerializable
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
            _saveSystem?.SaveObject(this);
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
            _saveSystem?.LoadObject(this);
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
        
        // TODO: Change key, `name` will be different when build release
        public string Key { get { return this.name; } }

        public string ToJson()
        {
            return JsonUtility.ToJson(_inventory);
        }

        public bool FromJson(string json)
        {
            try { JsonUtility.FromJsonOverwrite(json, _inventory); return true; } catch { }
            return false;
        }
        #endregion
    }
}