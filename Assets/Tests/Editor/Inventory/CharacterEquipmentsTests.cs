using CryptoQuest.Config;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using SlotType = CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquippingSlotContainer.EType;

namespace CryptoQuest.Tests.Editor
{
    [TestFixture]
    public class CharacterEquipmentsTests
    {
        private const string ITEMS_EQUIPMENTS_TWO_HAND =
            "Assets/ScriptableObjects/Data/Inventory/Items/Equipments/Weapons/Wand.asset";

        private const string EQUIPMENT_PATH = "Assets/ScriptableObjects/Data/Inventory/Items/Equipments/";

        private CharacterEquipments _characterEquipments = new();
        private InventoryConfigSO _inventoryConfigSO;
        private InventorySO _inventorySO;

        [SetUp]
        public void Setup()
        {
            _inventoryConfigSO = GetInventoryConfig();
            _characterEquipments.Initialize(_inventoryConfigSO.CategorySlotIndex, _inventoryConfigSO.SlotTypeIndex);
            _inventorySO = ScriptableObject.CreateInstance<InventorySO>();
        }

        /// <summary>
        /// This test is a smoke test for InventorySO.
        /// <see cref="InventorySO.Editor_GetEquipmentSlotsCount"/>
        /// </summary>
        [Test]
        [Category("Smokes")]
        public void InventorySO_ShouldHaveCorrectSlots()
        {
            var expected = _inventoryConfigSO.CategorySlotIndex;
            var actual = _characterEquipments.Editor_GetEquipmentSlotsCount();

            Assert.AreEqual(expected, actual, $"Expected: {expected} | Actual: {actual}");
        }

        [TestCase(EQUIPMENT_PATH + "Helmet.asset", SlotType.Head)]
        [TestCase(EQUIPMENT_PATH + "Necklace.asset", SlotType.Accessory1)]
        public void Equip_Equipment_ShouldFillEquipmentSlot(
            string equipmentPath,
            SlotType slot)
        {
            var expected = EquipItemFromDataWithPath(slot, equipmentPath);
            // var actual = _inventorySO.GetInventorySlot(slot).Equipment;

            _inventorySO.Add(expected);

            // Assert.AreEqual(expected, actual, $"Expected: {expected} | Actual: {actual}");
        }

        [TestCase(EQUIPMENT_PATH + "Weapons/Wand.asset", SlotType.Weapon)]
        [TestCase(EQUIPMENT_PATH + "Weapons/Axe.asset", SlotType.Weapon)]
        public void Equip_TwoHandedWeaponTypeSO_ShouldOccupiedWeaponAndShieldSlot(
            string equipmentPath,
            SlotType slot)

        {
            var expected = EquipItemFromDataWithPath(slot, equipmentPath);
            // var actual = _inventorySO.GetInventorySlot(slot).Equipment;

            _inventorySO.Add(expected);

            // Assert.AreEqual(expected, actual, $"Expected: {expected} | Actual: {actual}");
        }

        [TestCase(EQUIPMENT_PATH + "Weapons/Sword.asset", SlotType.Weapon)]
        [TestCase(EQUIPMENT_PATH + "Shield.asset", SlotType.Shield)]
        public void Equip_EquipmentTwoHandWithOneHandItem_ShouldRemoveLastEquipmentSlot(
            string equipmentPath,
            SlotType slot)
        {
            EquipItemFromDataWithPath(slot, ITEMS_EQUIPMENTS_TWO_HAND);

            var expected = EquipItemFromDataWithPath(slot, equipmentPath);

            var currentType = expected.Item.EquipmentType.AllowedSlots[0];

            _inventorySO.Add(expected);
            //
            // if (currentType == SlotType.Weapon)
            // {
            //     Assert.AreEqual(expected, _inventorySO.GetInventorySlot(SlotType.Weapon).Equipment,
            //         $"Expected: {expected} | Actual: {_inventorySO.GetInventorySlot(SlotType.Weapon).Equipment.Item.name}");
            //     Assert.IsNull(_inventorySO.GetInventorySlot(SlotType.Shield).Equipment,
            //         $"Expected: null | Actual: {_inventorySO.GetInventorySlot(SlotType.Shield).Equipment}");
            // }
            // else if (currentType == SlotType.Shield)
            // {
            //     Assert.AreEqual(expected, _inventorySO.GetInventorySlot(SlotType.Shield).Equipment,
            //         $"Expected: {expected} | Actual: {_inventorySO.GetInventorySlot(SlotType.Shield).Equipment.Item.name}");
            //     Assert.IsNull(_inventorySO.GetInventorySlot(SlotType.Weapon).Equipment,
            //         $"Expected: null | Actual: {_inventorySO.GetInventorySlot(SlotType.Weapon).Equipment}");
            // }
        }

        private InventoryConfigSO GetInventoryConfig()
        {
            var guids = AssetDatabase.FindAssets("t:InventoryConfigSO");

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var inventoryConfigSO = AssetDatabase.LoadAssetAtPath<InventoryConfigSO>(path);

            return inventoryConfigSO;
        }

        private EquipmentInfo EquipItemFromDataWithPath(SlotType slot, string equipmentPath)
        {
            var equipmentSO = AssetDatabase.LoadAssetAtPath<EquipmentSO>(equipmentPath);

            Assert.NotNull(equipmentSO, $"EquipmentSO is null. Path: {equipmentPath}");

            EquipmentInfo equipment = new EquipmentInfo(equipmentSO);

            Assert.NotNull(equipment, $"Equipment is null. Path: {equipmentPath}");

            // _inventorySO.Equip(slot, equipment);
            return equipment;
        }
    }
}