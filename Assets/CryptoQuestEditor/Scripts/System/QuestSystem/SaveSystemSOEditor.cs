using CryptoQuest.API;
using CryptoQuest.Networking;
using CryptoQuest.System.SaveSystem;
using CryptoQuest.System.SaveSystem.Savers;
using IndiGames.Core.Common;
using UniRx;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.System.QuestSystem
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

            var saveToServerButton = root.Q<Button>("save-to-server-button");
            var clearAllSaveButton = root.Q<Button>("clear-all-save-button");
            var openSaveFolderButton = root.Q<Button>("open-save-folder-button");

            saveToServerButton.clicked += SaveToServer;
            clearAllSaveButton.clicked += OnClearAllSaveButtonOnclicked;
            openSaveFolderButton.clicked += OnOpenSaveFolderButtonOnclicked;


            return root;
        }

        private void SaveToServer()
        {
            Target.Save();
            var restClient = ServiceProvider.GetService<IRestClient>();
            restClient?.WithBody(new OnlineProgressionSaver.SaveDataBody() { GameData = Target.SaveData })
                .Post<OnlineProgressionSaver.SaveDataResult>(Accounts.USER_SAVE_DATA).Subscribe(Saved);
        }

        private void Saved(OnlineProgressionSaver.SaveDataResult res)
        {
            Debug.Log($"Saved: {res.Code}");
            EditorUtility.SetDirty(Target);
            AssetDatabase.SaveAssets();
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
            SaveToServer();
        }
    }
}