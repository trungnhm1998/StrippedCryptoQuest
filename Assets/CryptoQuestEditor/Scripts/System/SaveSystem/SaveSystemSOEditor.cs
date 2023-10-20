using CryptoQuest.System.SaveSystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor
{
    [CustomEditor(typeof(SaveSystemSO))]
    public class SaveSystemSOEditor : Editor
    {
        [SerializeField] private VisualTreeAsset _uxml;
        private SaveSystemSO Target => target as SaveSystemSO;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _uxml.CloneTree(root);

            var clearAllSaveButton = root.Q<Button>("clear-all-save-button");
            var openSaveFolderButton = root.Q<Button>("open-save-folder-button");

            clearAllSaveButton.clicked += OnClearAllSaveButtonOnclicked;
            openSaveFolderButton.clicked += OnOpenSaveFolderButtonOnclicked;

            return root;
        }

        private void OnOpenSaveFolderButtonOnclicked() => Target.Editor_OpenSaveFolder();

        private void OnClearAllSaveButtonOnclicked()
        {
            Target.Editor_ClearSave();
            EditorUtility.SetDirty(Target);
        }
    }
}