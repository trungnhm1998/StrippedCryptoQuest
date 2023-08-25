using CryptoQuest.Config;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using SlotType = CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquipmentSlot.EType;

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
            _inventorySO = ScriptableObject.CreateInstance<InventorySO>();
        }

        private InventoryConfigSO GetInventoryConfig()
        {
            var guids = AssetDatabase.FindAssets("t:InventoryConfigSO");

            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var inventoryConfigSO = AssetDatabase.LoadAssetAtPath<InventoryConfigSO>(path);

            return inventoryConfigSO;
        }
    }
}