using IndiGames.Core.Database;
using UnityEditor;
using UnityEngine;

namespace IndiGamesEditor.Core
{
    [CustomEditor(typeof(AssetReferenceDatabaseT), true)]
    public class DatabaseEditor : Editor
    {
        private AssetReferenceDatabaseT Target => (AssetReferenceDatabaseT) target;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Load all data"))
            {
                Debug.Log($"{Target.GetAssetType()}");
                Target.Editor_FetchDataInProject();
                
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }
        }
    }
}