using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Item.Consumable;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Item
{
    public class ConsumableInventoryTests
    {
        private ConsumableInventory _inventory;
        private ConsumableSO _consumable;

        [SetUp]
        public void SetUp()
        {
            _inventory = ScriptableObject.CreateInstance<ConsumableInventory>();
            _consumable = ScriptableObject.CreateInstance<ConsumableSO>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_inventory);
            Object.DestroyImmediate(_consumable);
        }

        [Test]
        public void Add_IncreasesQuantity_WhenConsumableExists()
        {
            _inventory.Add(_consumable, 1);

            Assert.AreEqual(1, _inventory.Items[0].Quantity);
        }

        [Test]
        public void Add_CreatesNewItem_WhenConsumableDoesNotExist()
        {
            _inventory.Add(_consumable, 1);

            Assert.AreEqual(1, _inventory.Items.Count);
            Assert.AreEqual(_consumable, _inventory.Items[0].Data);
        }

        [Test]
        public void Add_DoesNothing_WhenQuantityIsZeroOrLess()
        {
            _inventory.Add(_consumable, 0);

            Assert.AreEqual(0, _inventory.Items.Count);
        }

        [Test]
        public void Remove_DecreasesQuantity_WhenConsumableExists()
        {
            _inventory.Add(_consumable, 2);
            _inventory.Remove(_consumable, 1);

            Assert.AreEqual(1, _inventory.Items[0].Quantity);
        }

        [Test]
        public void Remove_RemovesItem_WhenQuantityIsZeroOrLess()
        {
            _inventory.Add(_consumable, 1);
            _inventory.Remove(_consumable, 1);

            Assert.AreEqual(0, _inventory.Items.Count);
        }

        [Test]
        public void Remove_DoesNothing_WhenConsumableDoesNotExist()
        {
            _inventory.Remove(_consumable, 1);

            Assert.AreEqual(0, _inventory.Items.Count);
        }

        [Test]
        public void Contains_ReturnsTrue_WhenConsumableExists()
        {
            _inventory.Add(_consumable, 1);

            Assert.IsTrue(_inventory.Contains(_consumable));
        }

        [Test]
        public void Contains_ReturnsFalse_WhenConsumableDoesNotExist()
        {
            Assert.IsFalse(_inventory.Contains(_consumable));
        }
    }
}