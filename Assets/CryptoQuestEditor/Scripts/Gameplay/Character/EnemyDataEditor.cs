using CryptoQuest.Character.Enemy;
using CryptoQuest.Gameplay.Enemy;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
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

            var addDropButton = root.Q<Button>("add-drop-button");
            addDropButton.clicked += () => AddLoot();

            return root;
        }

        private void AddLoot()
        {
            Target.Editor_AddDrop(Target.DropToAdd.Loot);
            EditorUtility.SetDirty(Target);
        }
    }
}