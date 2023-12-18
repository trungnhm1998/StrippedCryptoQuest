using CryptoQuest.Actions;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
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
        private Button _removeButton;
        private InventorySO Target => target as InventorySO;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _uxml.CloneTree(root);

            _removeButton = root.Q<Button>("remove-all-button");
            var syncButton = root.Q<Button>("sync-button");

            syncButton.SetEnabled(Application.isPlaying);
            syncButton.clicked += SyncEquipments;
            _removeButton.clicked += RemoveAll;

            return root;
        }

        [MenuItem("Crypto Quest/Sync Equipments Inventory")]
        public static void SyncEquipments()
        {
            if (Application.isPlaying == false) return;
            ActionDispatcher.Dispatch(new FetchProfileEquipmentsAction());
        }
        
        [MenuItem("Crypto Quest/Sync Equipments Inventory", true)]
        public static bool ValidateSyncEquipments()
        {
            return Application.isPlaying;
        }

        private void RemoveAll()
        {

        }
    }
}