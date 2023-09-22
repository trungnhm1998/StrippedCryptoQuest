using CryptoQuest.Character.Hero;
using CryptoQuest.Item.Equipment;
using NUnit.Framework;

namespace CryptoQuest.Tests.Editor.Items
{
    [TestFixture]
    public class CharacterEquipmentsTests
    {
        [Test]
        public void GetEquipmentInSlot_WhenSlotIsEmpty_ReturnInvalidEquipment()
        {
            var equipments = new Equipments();
            
            var equipment = equipments.GetEquipmentInSlot(EquipmentSlot.EType.RightHand);
            
            Assert.IsFalse(equipment.IsValid());
        }
    }
}