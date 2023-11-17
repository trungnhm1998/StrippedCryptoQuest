using CryptoQuest.Quest;
using UnityEditor;

namespace CryptoQuestEditor.System.QuestSystem
{
    public class QuestSaveSOEditorWindow : EditorWindow
    {
        // ctrl + q
        [MenuItem("Crypto Quest/Delete Quest Save %q")]
        public static void ClearSaveQuestData()
        {
            var window = GetWindow<QuestSaveSOEditorWindow>();
            window.Close();
        }

        private void OnEnable()
        {
            QuestSaveSO target = GetFileQuestSave();
            if (target == null) return;

            target.InProgressQuest.Clear();
            target.CompletedQuests.Clear();

            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }

        private QuestSaveSO GetFileQuestSave()
        {
            string[] guid = AssetDatabase.FindAssets("t:QuestSaveSO");
            if (guid.Length == 0) return null;

            string path = AssetDatabase.GUIDToAssetPath(guid[0]);
            return AssetDatabase.LoadAssetAtPath<QuestSaveSO>(path);
        }
    }
}