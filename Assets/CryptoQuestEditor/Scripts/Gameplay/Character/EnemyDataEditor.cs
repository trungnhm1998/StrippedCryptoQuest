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

            var addEquipmentButton = root.Q<Button>("add-equipment-button");
            addEquipmentButton.clicked += () => AddLoot(new EquipmentLootInfo(new EquipmentInfo()));

            var addUsableItemButton = root.Q<Button>("add-consumable-button");
            addUsableItemButton.clicked += () => AddLoot(new UsableLootInfo(new ConsumableInfo()));

            var addCurrencyButton = root.Q<Button>("add-currency-button");
            addCurrencyButton.clicked += () => AddLoot(new CurrencyLootInfo(new CurrencyInfo()));

            var addExpButton = root.Q<Button>("add-xp-button");
            addExpButton.clicked += () => AddLoot(new ExpLoot(0));

            return root;
        }

        private void AddLoot(LootInfo loot)
        {
            Target.Editor_AddDrop(loot);
            EditorUtility.SetDirty(Target);
        }
    }
}