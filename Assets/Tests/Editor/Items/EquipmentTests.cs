using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Item.Equipment;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Items
{
    [TestFixture]
    public class EquipmentTests
    {
        [Test]
        public void Equals_WhenSameId_ReturnsTrue()
        {
            var equipment1 = new EquipmentInfo();
            var equipment2 = equipment1;
            var result = equipment1 == equipment2;
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_WhenDifferentId_ReturnsFalse()
        {
            var equipment1 = new EquipmentInfo();
            var equipment2 = new EquipmentInfo();
            var result = equipment1 == equipment2;
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_WhenSameId_ReturnsFalse()
        {
            var equipment1 = new EquipmentInfo();
            var equipment2 = equipment1;
            var result = equipment1 != equipment2;
            Assert.IsFalse(result);
        }

        [Test]
        public void Equals_WhenDifferentId_ReturnsTrue()
        {
            var equipment1 = new EquipmentInfo();
            var equipment2 = new EquipmentInfo();
            var result = equipment1 != equipment2;
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValid_WhenDataIsNull_ReturnsFalse()
        {
            var equipment = new EquipmentInfo();
            var result = equipment.IsValid();
            Assert.IsFalse(result);
        }
    }
}