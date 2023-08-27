using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using NUnit.Framework;

namespace CryptoQuest.Tests.Editor.Items
{
    [TestFixture]
    public class CharacterEquipmentsTests
    {
        [Test]
        public void GetEquipmentInSlot_WhenSlotIsEmpty_ReturnInvalidEquipment()
        {
            var equipments = new CharacterEquipments();
            
            var equipment = equipments.GetEquipmentInSlot(EquipmentSlot.EType.RightHand);
            
            Assert.IsFalse(equipment.IsValid());
        }
    }
}