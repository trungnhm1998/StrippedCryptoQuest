using System;
using CryptoQuest.Battle.Components;
using CryptoQuest.Item.Equipment;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using ESlotType = CryptoQuest.Item.Equipment.EquipmentSlot.EType;
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


        [TestCase(new[] { ESlotType.LeftHand }, new[] { ESlotType.LeftHand }, ESlotType.LeftHand)]
        [TestCase(null, new[] { ESlotType.Accessory1, ESlotType.Accessory2 }, ESlotType.Accessory1)]
        [TestCase(null, new[] { ESlotType.Accessory1, ESlotType.Accessory2 }, ESlotType.Accessory2)]
        public void Equip_ValidEquipment_EquipmentIsEquipped(
            ESlotType[] requiredSlots, ESlotType[] allowedSlots, ESlotType equippingSlot)
        {
            _equipment.IsValid().Returns(true);
            _equipment.RequiredSlots.Returns(requiredSlots ?? Array.Empty<ESlotType>());
            _equipment.AllowedSlots.Returns(allowedSlots);

            _equipmentsController.Equip(_equipment, equippingSlot);

            Assert.AreEqual(_equipment, _equipmentsController.GetEquipmentInSlot(equippingSlot));
        }

        [Test]
        public void Equip_InvalidEquipment_EquipmentIsNotEquipped()
        {
            _equipment.IsValid().Returns(false);
            _equipment.RequiredSlots.Returns(new[] { ESlotType.LeftHand });
            _equipment.AllowedSlots.Returns(new[] { ESlotType.LeftHand });

            _equipmentsController.Equip(_equipment, ESlotType.LeftHand);

            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlotType.LeftHand));
        }

        [Test]
        public void Equip_EquipmentNotAllowedInSlot_EquipmentIsNotEquipped()
        {
            _equipment.IsValid().Returns(true);
            _equipment.RequiredSlots.Returns(new[] { ESlotType.RightHand });
            _equipment.AllowedSlots.Returns(new[] { ESlotType.RightHand });

            _equipmentsController.Equip(_equipment, ESlotType.LeftHand);

            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlotType.LeftHand));
        }
        
        [Test]
        public void Equip_EquipmentRequiredSlots_SlotsAreOccupied()
        {
            _equipment.IsValid().Returns(true);
            _equipment.RequiredSlots.Returns(new[] { ESlotType.RightHand, ESlotType.LeftHand });
            _equipment.AllowedSlots.Returns(new[] { ESlotType.RightHand });

            _equipmentsController.Equip(_equipment, ESlotType.RightHand);

            Assert.AreEqual(_equipment, _equipmentsController.GetEquipmentInSlot(ESlotType.RightHand));
            Assert.AreEqual(_equipment, _equipmentsController.GetEquipmentInSlot(ESlotType.LeftHand));
        }
        
        [Test]
        public void Unequip_EquipmentIsEquipped_EquipmentIsUnequipped()
        {
            _equipment.IsValid().Returns(true);
            _equipment.RequiredSlots.Returns(new[] { ESlotType.RightHand, ESlotType.LeftHand });
            _equipment.AllowedSlots.Returns(new[] { ESlotType.RightHand });

            _equipmentsController.Equip(_equipment, ESlotType.RightHand);
            _equipmentsController.Unequip(_equipment);

            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlotType.RightHand));
            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlotType.LeftHand));
        }
        
        [Test]
        public void Unequip_EquipmentIsNotEquipped_EquipmentIsNotUnequipped()
        {
            _equipment.IsValid().Returns(true);
            _equipment.RequiredSlots.Returns(new[] { ESlotType.RightHand, ESlotType.LeftHand });
            _equipment.AllowedSlots.Returns(new[] { ESlotType.RightHand });

            _equipmentsController.Unequip(_equipment);

            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlotType.RightHand));
            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlotType.LeftHand));
        }
        
        [Test]
        public void Unequip_EquipmentRequiredSlots_SlotsAreOccupied()
        {
            _equipment.IsValid().Returns(true);
            _equipment.RequiredSlots.Returns(new[] { ESlotType.RightHand, ESlotType.LeftHand });
            _equipment.AllowedSlots.Returns(new[] { ESlotType.RightHand });

            _equipmentsController.Equip(_equipment, ESlotType.RightHand);
            _equipmentsController.Unequip(_equipment);

            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlotType.RightHand));
            Assert.IsNull(_equipmentsController.GetEquipmentInSlot(ESlotType.LeftHand));
        }
    }
}