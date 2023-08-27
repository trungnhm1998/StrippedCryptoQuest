using CryptoQuest.Config;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using SlotType = CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquipmentSlot.EType;

namespace CryptoQuest.Tests.Editor
{
    [TestFixture]
    public class InventorySOTests
    {
        private const string USABLE_PATH = "Assets/ScriptableObjects/Data/Inventory/Items/Usables/";
        private const string EQUIPMENT_PATH = "Assets/ScriptableObjects/Data/Inventory/Items/Equipments/";

        private InventorySO _inventorySO;
        private InventoryConfigSO _inventoryConfigSO;

        /// <summary>
        ///   This method is called before each test.
        /// <see cref="InventorySO.Editor_GetEquippingCache"/>
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _inventoryConfigSO = GetInventoryConfig();
            _inventorySO = ScriptableObject.CreateInstance<InventorySO>();
        }

        [TearDown]
        public void Destroy()
        {
            Object.DestroyImmediate(_inventorySO);
        }

        /// <summary>
        /// This test is a smoke test for InventorySO.
        /// <see cref="InventorySO.Editor_GetSubInventoryContainers"/>
        /// </summary>
        [Test]
        [Category("Smokes")]
        public void InventorySO_ShouldHaveCorrectCategoryInSubInventories()
        {
            var inventorySOs = GetAllInventorySO();

            foreach (var inventorySO in inventorySOs)
            {
                var values = inventorySO.Editor_GetSubInventoryContainers();

                foreach (var expected in values)
                {
                    Assert.AreEqual(expected.EquipmentCategory, (EEquipmentCategory)values.IndexOf(expected),
                        $"Expected: {expected.EquipmentCategory} | Actual: {values.IndexOf(expected)}");
                }
            }
        }

        /// <summary>
        /// This test is a smoke test for InventorySO.
        /// <see cref="InventorySO.Editor_GetInventorySlotsCacheCount"/>
        /// </summary>
        [Test]
        [Category("Smokes")]
        public void InventorySO_ShouldHaveCorrectInventorySlots()
        {
            var expected = _inventoryConfigSO.SlotTypeIndex;
            var actual = _inventorySO.Editor_GetInventorySlotsCacheCount();

            Assert.AreEqual(expected, actual, $"Expected: {expected} | Actual: {actual}");
        }


        [TestCase(USABLE_PATH + "Usable5002.asset")]
        [TestCase(USABLE_PATH + "Usable5003.asset")]
        [TestCase(USABLE_PATH + "Usable5004.asset")]
        [TestCase(USABLE_PATH + "Usable5005.asset")]
        public void Add_UsableItem_ShouldHaveOneItem(string usablePath)
        {
            UsableInfo item = NewUsable(usablePath, out UsableSO actual);

            _inventorySO.Add(item);
            var expected = _inventorySO.UsableItems[0].Data;

            Assert.AreEqual(expected, actual, $"Expected: {expected} | Actual: {actual}");
        }

        [TestCase(USABLE_PATH + "Usable5005.asset")]
        public void Add_UsableItem_ShouldReturnExactlyQuantity(string usablePath)
        {
            UsableInfo item = NewUsable(usablePath, out UsableSO actual);

            var actualQuantity = Random.Range(1, 100);

            _inventorySO.Add(item, actualQuantity);
            var expected = _inventorySO.UsableItems[0].Quantity;


            Assert.AreEqual(expected, actualQuantity, $"Expected: {expected} | Actual: {actualQuantity}");
        }

        [Test]
        public void Add_ItemWithOutData_ShouldReturnFalse()
        {
            var result = _inventorySO.Add(new UsableInfo());
            Assert.False(result);
        }

        [Test]
        public void Add_WithEquipmentButNoData_ShouldReturnFalse()
        {
            var result = _inventorySO.Add(new EquipmentInfo());
            Assert.False(result);
        }

        [Test]
        public void Add_NullEquipment_ShouldReturnFalse()
        {
            var result = _inventorySO.Add(null);
            Assert.False(result);
        }

        [Test]
        public void Remove_NullEquipment_ShouldReturnFalse()
        {
            var result = _inventorySO.Remove(null);
            Assert.False(result);
        }

        [Test]
        public void Remove_WithEquipmentWithOutData_ShouldReturnFalse()
        {
            var result = _inventorySO.Remove(new EquipmentInfo());
            Assert.False(result);
        }

        private UsableInfo NewUsable(string usablePath, out UsableSO item)
        {
            item = AssetDatabase.LoadAssetAtPath<UsableSO>(usablePath);
            return new UsableInfo(item);
        }

        private InventoryConfigSO GetInventoryConfig()
        {
            var guids = AssetDatabase.FindAssets("t:InventoryConfigSO");

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var inventoryConfigSO = AssetDatabase.LoadAssetAtPath<InventoryConfigSO>(path);

            return inventoryConfigSO;
        }

        private static InventorySO[] GetAllInventorySO()
        {
            var guids = AssetDatabase.FindAssets("t:InventorySO");

            Assert.Greater(guids.Length, 0, $"Expected: {guids.Length} | Actual: {guids.Length}");

            var inventorySOs = new InventorySO[guids.Length];

            Assert.Greater(inventorySOs.Length, 0, $"Expected: {inventorySOs.Length} | Actual: {inventorySOs.Length}");

            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var currentInventorySO = AssetDatabase.LoadAssetAtPath<InventorySO>(path);
                inventorySOs[i] = currentInventorySO;
            }

            Assert.IsNotEmpty(inventorySOs, $"Expected: {inventorySOs} | Actual: {inventorySOs}");

            return inventorySOs;
        }
    }
}