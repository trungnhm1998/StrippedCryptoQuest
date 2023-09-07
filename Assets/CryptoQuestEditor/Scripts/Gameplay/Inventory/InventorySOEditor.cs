using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuestEditor.Helper;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor
{
    [CustomEditor(typeof(InventorySO))]
    public class InventorySOEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _uxml;
        private Button _equipmentButton;
        private Button _usableItemButton;
        private Button _removeButton;

        private InventorySO Target => target as InventorySO;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _uxml.CloneTree(root);

            _equipmentButton = root.Q<Button>("add-equipment-button");
            _usableItemButton = root.Q<Button>("add-consumable-button");
            _removeButton = root.Q<Button>("remove-all-button");

            _equipmentButton.clicked += AddAllEquipment;
            _usableItemButton.clicked += AddAllUsableItem;
            _removeButton.clicked += RemoveAll;

            return root;
        }

        private void AddAllEquipment()
        {
            EquipmentSO[] allEquipment = ToolsHelper.GetAssets<EquipmentSO>();

            foreach (EquipmentSO equipment in allEquipment)
            {
                Target.Equipments.Add(new EquipmentInfo(equipment));
            }
        }

        private void AddAllUsableItem()
        {
            UsableSO[] allUsableItem = ToolsHelper.GetAssets<UsableSO>();

            foreach (UsableSO usableItem in allUsableItem)
            {
                Target.UsableItems.Add(new UsableInfo(usableItem));
            }
        }

        private void RemoveAll()
        {
            UsableInfo[] usableItems = Target.UsableItems.ToArray();
            EquipmentInfo[] equipmentItems = Target.Equipments.ToArray();

            foreach (var item in usableItems)
            {
                Target.Remove(item);
            }

            foreach (var item in equipmentItems)
            {
                Target.Remove(item);
            }
        }
    }
}