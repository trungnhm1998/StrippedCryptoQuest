using IndiGames.Core.Database;
using UnityEditor;
using UnityEngine;

namespace IndiGamesEditor.Core
{
    [CustomEditor(typeof(GenericAssetReferenceDatabase), true)]
    public class DatabaseEditor : Editor
    {
        private GenericAssetReferenceDatabase Target => (GenericAssetReferenceDatabase) target;
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