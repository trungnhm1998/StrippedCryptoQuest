using CryptoQuest.SaveSystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.SaveSystem
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

        private void OnOpenSaveFolderButtonOnclicked()
        {
            Application.OpenURL(Application.persistentDataPath);
        }

        private void OnClearAllSaveButtonOnclicked()
        {
            var so = new SerializedObject(Target);
            so.FindProperty("_saveData").boxedValue = new SaveData();
            so.ApplyModifiedProperties();
            Target.Save();
            EditorUtility.SetDirty(Target);
            AssetDatabase.SaveAssets();
        }
    }
}