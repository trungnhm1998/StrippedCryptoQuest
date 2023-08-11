using System.Collections;
using CryptoQuest.Events.Gameplay;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace CryptoQuest.Tests.Runtime.Inventory
{
    [TestFixture]
    public class InventoryControllerTests
    {
#if UNITY_EDITOR

        private const string ADD_ITEM_EVENT_CHANNEL = "Assets/ScriptableObjects/Items/OnAddItemEvent.asset";
        private const string REMOVE_ITEM_EVENT_CHANNEL = "Assets/ScriptableObjects/Items/OnRemoveItemEvent.asset";
        private const string EQUIP_EVENT_CHANNEL = "Assets/ScriptableObjects/Items/OnEquipEvent.asset";
        private const string UNEQUIP_EVENT_CHANNEL = "Assets/ScriptableObjects/Items/OnUnequipEvent.asset";

        private const string ITEM_TEST_PATH = "Assets/ScriptableObjects/Data/Inventory/Items/Usables/Usable5002.asset";

        private const string EQUIPMENT_DATA_PATH =
            "Assets/ScriptableObjects/Data/Inventory/Items/Equipments/Weapons/Sword.asset";

        private const string INVENTORY_TEST_PATH =
            "Assets/Tests/Runtime/Inventory/InventoryOnlyForIntergrationTest.asset";

        private const string INVENTORY_TEST_SCENE = "Assets/Tests/Runtime/Inventory/Inventory.unity";

        private InventorySO _inventorySO;
        private ItemEventChannelSO _onAddItem;
        private ItemEventChannelSO _onRemoveItem;

        private EquipmentEventChannelSO _onEquipItem;
        private EquipmentEventChannelSO _onUnequipItem;

        private EquipmentInfo _currentEquipmentInfo;
        private EquippingSlotContainer.EType _currentSlot;

        [UnitySetUp]
        public IEnumerator SetupOnTime()
        {
            _inventorySO = GetTestInventorySO();

            Assert.NotNull(_inventorySO, "InventorySO is null");

            _onAddItem = GetItemEvent(ADD_ITEM_EVENT_CHANNEL);

            Assert.NotNull(_onAddItem, "OnAddItemEvent is null");

            _onRemoveItem = GetItemEvent(REMOVE_ITEM_EVENT_CHANNEL);

            Assert.NotNull(_onRemoveItem, "OnRemoveItemEvent is null");

            _onEquipItem = GetEquipmentEvent(EQUIP_EVENT_CHANNEL);

            Assert.NotNull(_onEquipItem, "OnEquipItemEvent is null");

            _onUnequipItem = GetEquipmentEvent(UNEQUIP_EVENT_CHANNEL);

            Assert.NotNull(_onUnequipItem, "OnUnequipItemEvent is null");

            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(INVENTORY_TEST_SCENE,
                new LoadSceneParameters(LoadSceneMode.Single));
        }


        [TearDown]
        public void RemoveItem()
        {
            _inventorySO.UsableItems.Clear();
            _onUnequipItem.RaiseEvent(_currentSlot, _currentEquipmentInfo);
        }

        [UnityTest]
        public IEnumerator AddItem_ShouldHaveDataInInventory()
        {
            var item = AssetDatabase.LoadAssetAtPath<UsableSO>(ITEM_TEST_PATH);

            UsableInfo itemExpected = new UsableInfo(item);

            _onAddItem.RaiseEvent(itemExpected);

            Assert.AreEqual(1, _inventorySO.UsableItems.Count, "Inventory should have 1 item");

            yield return null;
        }

        [UnityTest]
        public IEnumerator RemoveItem_InventoryShouldBeEmpty()
        {
            var item = AssetDatabase.LoadAssetAtPath<UsableSO>(ITEM_TEST_PATH);

            UsableInfo itemExpected = new UsableInfo(item);

            _onAddItem.RaiseEvent(itemExpected);

            Assert.AreEqual(1, _inventorySO.UsableItems.Count, "Inventory should have 1 item");

            yield return new WaitForSeconds(1f);

            _onRemoveItem.RaiseEvent(itemExpected);

            Assert.AreEqual(0, _inventorySO.UsableItems.Count, "Inventory should be empty");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Equip_ShouldHaveExactEquipmentInSlot()
        {
            var data = AssetDatabase.LoadAssetAtPath<EquipmentSO>(EQUIPMENT_DATA_PATH);

            _currentEquipmentInfo = new EquipmentInfo(data);

            _onEquipItem.RaiseEvent(EquippingSlotContainer.EType.Weapon, _currentEquipmentInfo);

            yield return new WaitForSeconds(2f);

            Assert.AreEqual(_currentEquipmentInfo,
                _inventorySO.GetInventorySlot(EquippingSlotContainer.EType.Weapon).Equipment,
                "Inventory should have 1 item");
            yield return null;
        }

        [UnityTest]
        public IEnumerator Unequip_ShouldEmptyExactSlot()
        {
            var data = AssetDatabase.LoadAssetAtPath<EquipmentSO>(EQUIPMENT_DATA_PATH);

            _currentEquipmentInfo = new EquipmentInfo(data);

            _onEquipItem.RaiseEvent(EquippingSlotContainer.EType.Weapon, _currentEquipmentInfo);

            Assert.AreEqual(_currentEquipmentInfo,
                _inventorySO.GetInventorySlot(EquippingSlotContainer.EType.Weapon).Equipment,
                "Inventory should have 1 item");

            yield return new WaitForSeconds(2f);

            _onUnequipItem.RaiseEvent(EquippingSlotContainer.EType.Weapon, _currentEquipmentInfo);

            Assert.IsNull(_inventorySO.GetInventorySlot(EquippingSlotContainer.EType.Weapon).Equipment,
                "Inventory should be empty");

            yield return null;
        }

        private InventorySO GetTestInventorySO()
        {
            var result = AssetDatabase.LoadAssetAtPath<InventorySO>(INVENTORY_TEST_PATH);

            return result;
        }

        private EquipmentEventChannelSO GetEquipmentEvent(string path)
        {
            var currentEvent = AssetDatabase.LoadAssetAtPath<EquipmentEventChannelSO>(path);
            return currentEvent;
        }

        private ItemEventChannelSO GetItemEvent(string path)
        {
            var currentEvent = AssetDatabase.LoadAssetAtPath<ItemEventChannelSO>(path);
            return currentEvent;
        }
#endif
    }
}