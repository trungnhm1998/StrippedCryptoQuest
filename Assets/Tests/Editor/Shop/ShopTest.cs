using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Shop;
using CryptoQuest.Shop.UI.Panels.Item;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Shop
{
    [TestFixture]
    public class ShopTest
    {
        private const string INVENTORY_PATH = "Assets/ScriptableObjects/Inventories/TestInventory.asset";
        private InventorySO _inventorySO;

        [SetUp]
        public void Setup()
        {
            _inventorySO = AssetDatabase.LoadAssetAtPath<InventorySO>(INVENTORY_PATH);
        }

        [Test]
        public void GetItemInWeaponShop_WithInventory_ReturnWeaponAndNonNFTItem()
        {
            var items = _inventorySO.GetWeapons();

            foreach (var item in items)
            {
                Assert.AreEqual(item.Data.EquipmentCategory, EEquipmentCategory.Weapon, "Expected:Weapon Item");
                Assert.IsTrue(!item.IsNftItem, "Expected:Non NFT Item");
            }
        }

        [Test]
        public void GetItemInArmorShop_WithInventory_ReturnArmorAndNonNFTItem() // Armor is all item in inventory except weapon and usable
        {
            var items = _inventorySO.GetNonWeapons();

            foreach (var item in items)
            {
                Assert.AreNotEqual(item.Data.EquipmentCategory, EEquipmentCategory.Weapon, "Expected: Armor Item");
                Assert.IsTrue(!item.IsNftItem, "Expected:Non NFT Item");
            }
        }

        [Test]
        public void GetItemInUsableShop_WithInventory_ReturnUsableItemAndNotKeyItem()
        {
            var items = _inventorySO.GetConsumables();

            foreach (var item in items)
            {
                Assert.AreNotEqual(item.Data.consumableType, EConsumeable.Key, "Expected: Usable item except key item");
            }
        }
    }
}
