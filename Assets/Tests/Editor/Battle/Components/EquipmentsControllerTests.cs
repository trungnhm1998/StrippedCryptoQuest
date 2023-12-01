using System;
using CryptoQuest.Battle.Components;
using CryptoQuest.Item.Equipment;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CryptoQuest.Tests.Editor.Battle.Components
{
    [TestFixture]
    public class EquipmentsControllerTests
    {
        private EquipmentsController _equipmentsController;
        private IEquipment _equipment;
        private const string HERO_GAS_ASSET_PATH = "Assets/Prefabs/Battle/HeroGAS.prefab";

        [SetUp]
        public void SetUp()
        {
            _equipment = Substitute.For<IEquipment>();
            var heroPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(HERO_GAS_ASSET_PATH);
            var hero = Object.Instantiate(heroPrefab).GetComponent<HeroBehaviour>();
            _equipmentsController = hero.GetComponent<EquipmentsController>();
            _equipmentsController.Init();
        }


        [TestCase(new[] { ESlot.LeftHand }, new[] { ESlot.LeftHand }, ESlot.LeftHand)]
        [TestCase(null, new[] { ESlot.Accessory1, ESlot.Accessory2 }, ESlot.Accessory1)]
        [TestCase(null, new[] { ESlot.Accessory1, ESlot.Accessory2 }, ESlot.Accessory2)]
        public void Equip_ValidEquipment_EquipmentIsEquipped(
            ESlot[] requiredSlots, ESlot[] allowedSlots, ESlot equippingSlot)
        {
            _equipment.IsValid().Returns(true);
            _equipment.RequiredSlots.Returns(requiredSlots ?? Array.Empty<ESlot>());
            _equipment.AllowedSlots.Returns(allowedSlots);

            _equipmentsController.Equip(_equipment, equippingSlot);

            Assert.AreEqual(_equipment, _equipmentsController.GetEquipmentInSlot(equippingSlot));
        }

        [Test]
        public void Equip_InvalidEquipment_EquipmentIsNotEquipped()
        {
            _equipment.IsValid().Returns(false);
            _equipment.RequiredSlots.Returns(new[] { ESlot.LeftHand });
            _equipment.AllowedSlots.Returns(new[] { ESlot.LeftHand });

            _equipmentsController.Equip(_equipment, ESlot.LeftHand);

            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlot.LeftHand));
        }

        [Test]
        public void Equip_EquipmentNotAllowedInSlot_EquipmentIsNotEquipped()
        {
            _equipment.IsValid().Returns(true);
            _equipment.RequiredSlots.Returns(new[] { ESlot.RightHand });
            _equipment.AllowedSlots.Returns(new[] { ESlot.RightHand });

            _equipmentsController.Equip(_equipment, ESlot.LeftHand);

            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlot.LeftHand));
        }

        [Test]
        public void Equip_EquipmentRequiredSlots_SlotsAreOccupied()
        {
            _equipment.IsValid().Returns(true);
            _equipment.RequiredSlots.Returns(new[] { ESlot.RightHand, ESlot.LeftHand });
            _equipment.AllowedSlots.Returns(new[] { ESlot.RightHand });

            _equipmentsController.Equip(_equipment, ESlot.RightHand);

            Assert.AreEqual(_equipment, _equipmentsController.GetEquipmentInSlot(ESlot.RightHand));
            Assert.AreEqual(_equipment, _equipmentsController.GetEquipmentInSlot(ESlot.LeftHand));
        }

        [Test]
        public void Unequip_EquipmentIsEquipped_EquipmentIsUnequipped()
        {
            _equipment.IsValid().Returns(true);
            _equipment.RequiredSlots.Returns(new[] { ESlot.RightHand, ESlot.LeftHand });
            _equipment.AllowedSlots.Returns(new[] { ESlot.RightHand });

            _equipmentsController.Equip(_equipment, ESlot.RightHand);
            _equipmentsController.Unequip(_equipment);

            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlot.RightHand));
            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlot.LeftHand));
        }

        [Test]
        public void Unequip_EquipmentIsNotEquipped_EquipmentIsNotUnequipped()
        {
            _equipment.IsValid().Returns(true);
            _equipment.RequiredSlots.Returns(new[] { ESlot.RightHand, ESlot.LeftHand });
            _equipment.AllowedSlots.Returns(new[] { ESlot.RightHand });

            _equipmentsController.Unequip(_equipment);

            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlot.RightHand));
            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlot.LeftHand));
        }

        [Test]
        public void Unequip_EquipmentRequiredSlots_SlotsAreOccupied()
        {
            _equipment.IsValid().Returns(true);
            _equipment.RequiredSlots.Returns(new[] { ESlot.RightHand, ESlot.LeftHand });
            _equipment.AllowedSlots.Returns(new[] { ESlot.RightHand });

            _equipmentsController.Equip(_equipment, ESlot.RightHand);
            _equipmentsController.Unequip(_equipment);

            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlot.RightHand));
            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlot.LeftHand));
        }

        [Test]
        public void Equip_WithOccupiedLeftHand_ShouldUnequipLeftHandFirst()
        {
            var leftHandEquipment = Substitute.For<IEquipment>();
            leftHandEquipment.IsValid().Returns(true);
            leftHandEquipment.RequiredSlots.Returns(new[] { ESlot.LeftHand });
            leftHandEquipment.AllowedSlots.Returns(new[] { ESlot.LeftHand });

            _equipmentsController.Equip(leftHandEquipment, ESlot.LeftHand);
            Assert.AreEqual(leftHandEquipment, _equipmentsController.GetEquipmentInSlot(ESlot.LeftHand));

            var dualWieldingEquipment = Substitute.For<IEquipment>();
            dualWieldingEquipment.IsValid().Returns(true);
            dualWieldingEquipment.RequiredSlots.Returns(new[] { ESlot.LeftHand, ESlot.RightHand });
            dualWieldingEquipment.AllowedSlots.Returns(new[] { ESlot.RightHand });

            var hasRemovedLeftHand = false;
            _equipmentsController.Removed += (equipment) =>
            {
                if (equipment == leftHandEquipment) hasRemovedLeftHand = true;
            };

            _equipmentsController.Equip(dualWieldingEquipment, ESlot.RightHand);

            Assert.AreEqual(dualWieldingEquipment, _equipmentsController.GetEquipmentInSlot(ESlot.LeftHand));
            Assert.True(hasRemovedLeftHand);
        }
    }
}