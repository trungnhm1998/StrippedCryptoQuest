using CryptoQuest.Battle.Components;
using CryptoQuest.Item.Equipment;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace CryptoQuest.Tests.Editor.Item
{
    [TestFixture]
    public class EquipmentsControllerTests
    {
        [Test]
        public void Equip_EquipmentRequiredLeftHandSlot_ShouldOccupiedLeftHandSlot()
        {
            var equipmentsController = new GameObject().AddComponent<EquipmentsController>();

            var equipment = Substitute.For<EquipmentInfo>();
            equipment.RequiredSlots.Returns(new[] { EquipmentSlot.EType.LeftHand });
            equipment.AllowedSlots.Returns(new[] { EquipmentSlot.EType.LeftHand });
            equipmentsController.Equip(equipment, EquipmentSlot.EType.LeftHand);
        }
    }
}