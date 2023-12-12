using CryptoQuest.Actions;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item.Consumable;
using CryptoQuestEditor.Helper;
using IndiGames.Core.Events;
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
        private Button _usableItemButton;
        private Button _removeButton;
        private InventorySO Target => target as InventorySO;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _uxml.CloneTree(root);

            _usableItemButton = root.Q<Button>("add-consumable-button");
            _removeButton = root.Q<Button>("remove-all-button");
            var syncButton = root.Q<Button>("sync-button");

            syncButton.SetEnabled(Application.isPlaying);
            syncButton.clicked += SyncEquipments;
            _usableItemButton.clicked += AddAllUsableItem;
            _removeButton.clicked += RemoveAll;

            return root;
        }

        private void SyncEquipments()
        {
            ActionDispatcher.Dispatch(new FetchProfileEquipmentsAction());
        }

        private void AddAllUsableItem()
        {
            ConsumableSO[] allUsableItem = ToolsHelper.GetAssets<ConsumableSO>();

            foreach (ConsumableSO usableItem in allUsableItem)
            {
                Target.Consumables.Add(new ConsumableInfo(usableItem, 99));
            }
        }

        private void RemoveAll()
        {

        }
    }
}