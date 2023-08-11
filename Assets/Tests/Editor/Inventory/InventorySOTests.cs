using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using SlotType = CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquippingSlotContainer.EType;

namespace CryptoQuest.Tests.Editor
{
    [TestFixture]
    public class InventorySOTests
    {
        private const string ITEMS_EQUIPMENTS_TWO_HAND =
            "Assets/ScriptableObjects/Data/Inventory/Items/Equipments/Weapons/Wand.asset";

        private InventorySO _inventorySO;

        /// <summary>
        ///   This method is called before each test.
        /// <see cref="InventorySO.Editor_GetEquippingCache"/>
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _inventorySO = ScriptableObject.CreateInstance<InventorySO>();

            foreach (var slot in _inventorySO.Editor_GetEquippingCache())
            {
                slot.Value.Equipment = null;
            }
        }

        [TearDown]
        public void Destroy()
        {
            Object.DestroyImmediate(_inventorySO);
        }

        /// <summary>
        /// This test is a smoke test for InventorySO. <see cref="InventorySO.Editor_GetEquipmentSlotsCount"/>
        /// </summary>
        [Test]
        [Category("Smokes")]
        public void InventorySO_ShouldHaveCorrectSlots()
        {
            var expected = InventorySO.EQUIPMENT_SLOTS_COUNT;
            var actual = _inventorySO.Editor_GetEquipmentSlotsCount();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This test is a smoke test for InventorySO. <see cref="InventorySO.Editor_GetInventorySlotsCacheCount"/>
        /// </summary>
        [Test]
        [Category("Smokes")]
        public void InventorySO_ShouldHaveCorrectInventorySlots()
        {
            var expected = InventorySO.INVENTORY_SLOTS_COUNT;
            var actual = _inventorySO.Editor_GetInventorySlotsCacheCount();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This test is a smoke test for InventorySO. <see cref="InventorySO.Editor_GetSubInventoryContainers"/>
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
                    Assert.AreEqual(expected.EquipmentCategory, (EEquipmentCategory)values.IndexOf(expected));
                }
            }
        }

        /// <summary>
        /// This test is a smoke test for InventorySO. <see cref="InventorySO.Editor_GetEquippingSlotContainers"/>
        /// </summary>
        [Test]
        [Category("Smokes")]
        public void InventorySO_ShouldHaveCorrectCategoryAndType()
        {
            var inventorySOs = GetAllInventorySO();

            foreach (var inventorySO in inventorySOs)
            {
                var values = inventorySO.Editor_GetEquippingSlotContainers();

                foreach (var expected in values)
                {
                    EEquipmentCategory actualCategory = (EEquipmentCategory)Mathf.Clamp(values.IndexOf(expected), 0,
                        InventorySO.INVENTORY_SLOTS_COUNT - 1);

                    Assert.AreEqual(expected.EquipmentCategory, actualCategory);
                    Assert.AreEqual(expected.Type, (SlotType)values.IndexOf(expected));
                }
            }
        }

        [TestCase("Assets/ScriptableObjects/Data/Inventory/Items/Equipments/Helmet.asset", SlotType.Head)]
        [TestCase("Assets/ScriptableObjects/Data/Inventory/Items/Equipments/Necklace.asset", SlotType.Accessory1)]
        public void Equip_Equipment_ShouldFillEquipmentSlot(
            string equipmentPath,
            SlotType slot)
        {
            var expected = NewEquipmentFromDataWithPath(slot, equipmentPath);
            var actual = _inventorySO.GetInventorySlot(slot).Equipment;

            Assert.AreEqual(expected, actual);
        }

        [TestCase("Assets/ScriptableObjects/Data/Inventory/Items/Equipments/Weapons/Wand.asset", SlotType.Weapon)]
        [TestCase("Assets/ScriptableObjects/Data/Inventory/Items/Equipments/Weapons/Axe.asset", SlotType.Weapon)]
        public void Equip_TwoHandedWeaponTypeSO_ShouldOccupiedWeaponAndShieldSlot(
            string equipmentPath,
            SlotType slot)

        {
            var expected = NewEquipmentFromDataWithPath(slot, equipmentPath);
            var actual = _inventorySO.GetInventorySlot(slot).Equipment;

            Assert.AreEqual(expected, actual);
        }


        [TestCase("Assets/ScriptableObjects/Data/Inventory/Items/Equipments/Weapons/Sword.asset", SlotType.Weapon)]
        [TestCase("Assets/ScriptableObjects/Data/Inventory/Items/Equipments/Shield.asset", SlotType.Shield)]
        public void Equip_EquipmentTwoHandWithOneHandItem_ShouldRemoveLastEquipmentSlot(
            string equipmentPath,
            SlotType slot)
        {
            NewEquipmentFromDataWithPath(slot, ITEMS_EQUIPMENTS_TWO_HAND);

            var expected = NewEquipmentFromDataWithPath(slot, equipmentPath);

            var currentType = expected.Item.EquipmentType.AllowedSlots[0];

            if (currentType == SlotType.Weapon)
            {
                Assert.AreEqual(expected, _inventorySO.GetInventorySlot(SlotType.Weapon).Equipment);
                Assert.IsNull(_inventorySO.GetInventorySlot(SlotType.Shield).Equipment);
            }
            else if (currentType == SlotType.Shield)
            {
                Assert.AreEqual(expected, _inventorySO.GetInventorySlot(SlotType.Shield).Equipment);
                Assert.IsNull(_inventorySO.GetInventorySlot(SlotType.Weapon).Equipment);
            }
        }


        [TestCase("Assets/ScriptableObjects/Data/Inventory/Items/Equipments/Weapons/Sword.asset", SlotType.Weapon)]
        public void Add_WithEquipment_ShouldAddToInventory(string equipmentPath,
            EquippingSlotContainer.EType expectedSlot)
        {
            var equipment = NewEquipment(equipmentPath, out EquipmentSO data);

            var expected = _inventorySO.Add(equipment);

            Assert.True(expected);

            _inventorySO.GetEquipmentByType(data.EquipmentType.EquipmentCategory, out var equipments);
            Assert.IsNotEmpty(equipments);
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

        private EquipmentInfo NewEquipment(string equipmentPath, out EquipmentSO equipmentSO)
        {
            equipmentSO = AssetDatabase.LoadAssetAtPath<EquipmentSO>(equipmentPath);

            EquipmentInfo equipment = new EquipmentInfo(equipmentSO);

            return equipment;
        }

        private EquipmentInfo NewEquipmentFromDataWithPath(SlotType slot, string equipmentPath)
        {
            var equipmentSO = AssetDatabase.LoadAssetAtPath<EquipmentSO>(equipmentPath);

            EquipmentInfo equipment = new EquipmentInfo(equipmentSO);

            _inventorySO.Equip(slot, equipment);
            return equipment;
        }

        private static InventorySO[] GetAllInventorySO()
        {
            var guids = AssetDatabase.FindAssets("t:InventorySO");

            Assert.Greater(guids.Length, 0);

            var inventorySOs = new InventorySO[guids.Length];

            Assert.Greater(inventorySOs.Length, 0);

            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var currentInventorySO = AssetDatabase.LoadAssetAtPath<InventorySO>(path);
                inventorySOs[i] = currentInventorySO;
            }

            Assert.IsNotEmpty(inventorySOs);

            return inventorySOs;
        }
    }
}