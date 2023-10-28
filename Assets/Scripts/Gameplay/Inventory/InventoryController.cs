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

        bool Add(EquipmentInfo equipment);

        bool Remove(EquipmentInfo equipment);

        bool Add(ConsumableInfo consumable);

        bool Remove(ConsumableInfo consumable);
    }

    public class InventoryController : SaveObject, IInventoryController
    {
        [SerializeField] private InventorySO _inventory;
        public InventorySO Inventory => _inventory;

        [Header("Listening to")]
        [SerializeField] private LootEventChannelSO _addLootRequestEventChannel;

        private IEquipmentDefProvider _definitionDatabase;

        private void Awake()
        {
            ServiceProvider.Provide<IInventoryController>(this);
            _definitionDatabase = GetComponent<IEquipmentDefProvider>();
        }

        private IEnumerator LoadAllEquipment()
        {
            yield return WaitUntilTrue(IsLoaded);

            for (var index = 0; index < _inventory.Equipments.Count; index++)
            {
                var equipment = _inventory.Equipments[index];
                yield return _definitionDatabase.Load(equipment);
                _inventory.Equipments[index] = equipment;
            }

            _inventory.OnLoaded();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _addLootRequestEventChannel.EventRaised += AddLoot;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _addLootRequestEventChannel.EventRaised -= AddLoot;
        }

        private void Start()
        {
            StartCoroutine(LoadAllEquipment());
        }

        private void AddLoot(LootInfo loot)
        {
            loot.AddItemToInventory(_inventory);
            SaveSystem?.SaveObject(this);
        }

        public bool Add(EquipmentInfo equipment)
        {
            if (!_inventory.Add(equipment)) return false;
            SaveSystem?.SaveObject(this);
            return true;
        }

        public bool Remove(EquipmentInfo equipment)
        {
            if (!_inventory.Remove(equipment)) return false;
            SaveSystem?.SaveObject(this);
            return true;
        }

        public bool Add(ConsumableInfo consumable)
        {
            if (!_inventory.Add(consumable)) return false;
            SaveSystem?.SaveObject(this);
            return true;
        }

        public bool Remove(ConsumableInfo consumable)
        {
            var result = _inventory.Remove(consumable);
            SaveSystem?.SaveObject(this);
            return result;
        }

        #region SaveSystem

        public override string Key
        {
            get { return "Inventory"; }
        }

        public override string ToJson()
        {
            return _inventory.ToJson();
        }

        public override IEnumerator CoFromJson(string json)
        {
            yield return _inventory.CoFromJson(json);
        }
        #endregion
    }
}