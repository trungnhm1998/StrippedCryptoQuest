using CryptoQuest.SaveSystem;
using UnityEditor;

namespace CryptoQuestEditor.System.QuestSystem
{
    public class SaveSystemSOEditorWindow : EditorWindow
    {
        // ctrl + shift + d
        [MenuItem("Crypto Quest/Delete Save File %#d")]
        public static void ClearAllSaveQuestData()
        {
            var window = GetWindow<SaveSystemSOEditorWindow>();
            window.Close();
        }

        private void OnEnable()
        {
            var target = GetFileSave();
            if (target == null) return;

            var so = new SerializedObject(target);
            so.FindProperty("_saveData").boxedValue = new SaveData();
            so.ApplyModifiedProperties();
            target.Save();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();

            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }

        private SaveSystemSO GetFileSave()
        {
            var guid = AssetDatabase.FindAssets("t:SaveSystemSO");
            if (guid.Length == 0) return null;

            var path = AssetDatabase.GUIDToAssetPath(guid[0]);
            return AssetDatabase.LoadAssetAtPath<SaveSystemSO>(path);
        }
    }
}