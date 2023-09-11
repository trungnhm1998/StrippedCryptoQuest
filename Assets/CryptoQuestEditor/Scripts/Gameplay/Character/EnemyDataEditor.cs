using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.Items;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.Gameplay.Character
{
    [CustomEditor(typeof(EnemyDef))]
    public class EnemyDataEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _uxml;

        private EnemyDef Target => target as EnemyDef;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _uxml.CloneTree(root);

            // add-equipment button
            var addEquipmentButton = root.Q<Button>("add-equipment-button");
            addEquipmentButton.clicked += () =>
            {
                Target.Editor_AddDrop(new EquipmentInfo());
                EditorUtility.SetDirty(Target);
            };

            var addUsableItemButton = root.Q<Button>("add-consumable-button");
            addUsableItemButton.clicked += () =>
            {
                Target.Editor_AddDrop(new UsableInfo());
                EditorUtility.SetDirty(Target);
            };
            var addCurrencyButton = root.Q<Button>("add-currency-button");
            addCurrencyButton.clicked += () =>
            {
                Target.Editor_AddDrop(new CurrencyInfo());
                EditorUtility.SetDirty(Target);
            };


            return root;
        }
    }
}