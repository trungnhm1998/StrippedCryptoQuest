using System.Collections;
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
    [CustomEditor(typeof(ConsumableSO))]
    public class UsableSOEditor : Editor
    {
        private const string MAIN_INVENTORY = "MainInventory";

        [SerializeField] private VisualTreeAsset _uxml;
        private ConsumableSO Target => target as ConsumableSO;
        private InventorySO InventorySO { get; set; }

        private HelpBox _helpBox;
        private ObjectField _inventoryField;
        private Button _addButton;

        private void OnEnable()
        {
            InventorySO = ToolsHelper.GetAsset<InventorySO>(MAIN_INVENTORY);
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _uxml.CloneTree(root);

            _helpBox = root.Q<HelpBox>("help-box");
            _inventoryField = root.Q<ObjectField>("inventory-field");
            _addButton = root.Q<Button>("add-button");

            Notification();

            _inventoryField.RegisterValueChangedCallback(Notification);
            _addButton.clicked += AddEquipment;

            return root;
        }

        private void Notification(ChangeEvent<Object> evt = null)
        {
            if (_inventoryField.value == null || evt?.newValue == null)
            {
                _helpBox.messageType = HelpBoxMessageType.Warning;
                _helpBox.text = "If you don't set inventory, default inventory will be MainInventory";
            }
            else
            {
                InventorySO = (InventorySO)_inventoryField.value;

                _helpBox.messageType = HelpBoxMessageType.Info;
                _helpBox.text =
                    $"The current inventory is {InventorySO.name}, {Target.name} will be added into this inventory";
            }
        }

        private void AddEquipment()
        {
            InventorySO.Consumables.Add(new ConsumableInfo(Target));
        }
    }
}