using System;
using IndiGames.Core.Database;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace IndiGamesEditor.Core.Database
{
    [CustomEditor(typeof(AssetReferenceDatabaseT), true)]
    public class AssetReferenceDatabaseEditor : Editor
    {
        private AssetReferenceDatabaseT Target => (AssetReferenceDatabaseT) target;
        private SerializedProperty _plugins;
        private ReorderableList _pluginList;

        private void OnEnable()
        {
            _plugins = serializedObject.FindProperty("_plugins");
        }

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
            

            if (GUILayout.Button("Create Plugin"))
            {
                // var plugin = Activator.CreateInstance<GoogleSheetsPlugin>();
                // _plugin.managedReferenceValue = plugin;
                // serializedObject.ApplyModifiedProperties();
            }
        }
    }
}