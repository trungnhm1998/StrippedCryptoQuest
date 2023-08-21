using System;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Attribute = CryptoQuest.Gameplay.Inventory.Equipment.Attribute;

namespace CryptoQuestEditor.Inventory
{
    [CustomEditor(typeof(EquipmentSO))]
    public class EquipmentSOEditor : Editor
    {
        public VisualTreeAsset AttributeEditorUxml;

        private EquipmentSO Target => (EquipmentSO)target;

        private SerializedProperty _stats;

        private void OnEnable()
        {
            _stats = serializedObject.FindProperty("_stats");
            // update when _stats changed
        }

        public override VisualElement CreateInspectorGUI()
        {
            serializedObject.Update();
            
            VisualElement root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            AttributeEditorUxml.CloneTree(root);

            var multiColumn = root.Q<MultiColumnListView>();
            multiColumn.itemsSource = Target.Stats.Attributes;

            var cols = multiColumn.columns;
            cols["attribute"].makeCell = () => new ObjectField() { objectType = typeof(AttributeScriptableObject) };
            cols["min-value"].makeCell = () => new FloatField();
            cols["max-value"].makeCell = () => new FloatField();

            cols["attribute"].bindCell = (element, i) =>
            {
                var objectField = ((ObjectField)element);
                // objectField.bindingPath = $"Stats.Attributes.Array.data[{i}].AttributeDef";
                // objectField.Bind(serializedObject);
                objectField.value = Target.Stats.Attributes[i].AttributeDef;
                objectField.RegisterValueChangedCallback(evt =>
                {
                    Target.Stats.Attributes[i].AttributeDef = (AttributeScriptableObject)evt.newValue;
                });
            };

            cols["min-value"].bindCell = (element, i) =>
            {
                var floatField = ((FloatField)element);
                // bind
                floatField.bindingPath = $"Stats.Attributes.Array.data[{i}].MinValue";
                floatField.Bind(serializedObject);
                // set value
                floatField.value = Target.Stats.Attributes[i].MinValue;
                floatField.RegisterValueChangedCallback((evt =>
                {
                    Target.Stats.Attributes[i].MinValue = evt.newValue;
                }));
            };

            cols["max-value"].bindCell = (element, i) =>
            {
                var floatField = ((FloatField)element);
                floatField.value = Target.Stats.Attributes[i].MaxValue;
                floatField.RegisterValueChangedCallback((evt =>
                {
                    Target.Stats.Attributes[i].MaxValue = evt.newValue;
                }));
            };


            cols["attribute"].unbindCell = (element, i) => element.Unbind();
            cols["min-value"].unbindCell = (element, i) => element.Unbind();
            cols["max-value"].unbindCell = (element, i) => element.Unbind();

            _stats.serializedObject.ApplyModifiedProperties();
            
            return root;
        }
    }
}