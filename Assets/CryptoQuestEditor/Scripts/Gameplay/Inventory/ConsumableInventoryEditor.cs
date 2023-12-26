using CryptoQuest.Actions;
using CryptoQuest.Inventory;
using CryptoQuest.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Item.Consumable;
using CryptoQuestEditor.Helper;
using IndiGames.Core.Events;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor
{
    [CustomEditor(typeof(ConsumableInventory))]
    public class ConsumableInventoryEditor : Editor
    {
        private ConsumableInventory Target => target as ConsumableInventory;
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            // add all button
            var addAllButton = new Button(() =>
            {
                var inventory = target as ConsumableInventory;
                var allConsumables = ToolsHelper.GetAssets<ConsumableSO>();
                foreach (var consumable in allConsumables)
                {
                    Target.Add(consumable, consumable.Type == EConsumableType.Consumable ? 99 : 1);
                }
            })
            {
                text = "Add All"
            };

            root.Add(addAllButton);
            
            var syncButton = new Button(() =>
            {
                if (Application.isPlaying == false) return;
                ActionDispatcher.Dispatch(new FetchProfileConsumablesAction());
            })
            {
                text = "Sync"
            };
            
            syncButton.SetEnabled(Application.isPlaying);
            root.Add(syncButton);

            return root;
        }
    }
}