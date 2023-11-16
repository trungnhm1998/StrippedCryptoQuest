using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Quest.Authoring;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuest.Quest.Editor
{
    [CustomEditor(typeof(QuestSO),true)]
    public class QuestSOEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset _uxml;
        private QuestSO Target => target as QuestSO;

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

            var clearButton = root.Q<Button>("clear-button");
            clearButton.clicked += () => Target.Editor_ClearReward();
            
            return root;
        }

        private void AddLoot(LootInfo loot)
        {
            Target.Editor_AddReward(loot);
            EditorUtility.SetDirty(Target);
        }
    }
}