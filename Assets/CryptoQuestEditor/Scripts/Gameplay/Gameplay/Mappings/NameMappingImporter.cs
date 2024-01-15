using System.Globalization;
using System.IO;
using CryptoQuest.Mappings;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.CryptoQuestEditor.Scripts.Gameplay.Gameplay.Mappings
{
    public class NameMappingImporter : ScriptableObject
    {
        [SerializeField] private TextAsset _masterData;
        [SerializeField] private string _exportPath;
        [SerializeField] private string _databaseName;


        [ContextMenu("Import")]
        public void Import()
        {
            if (string.IsNullOrEmpty(_exportPath))
                SelectExportPath();

            var csvPath = AssetDatabase.GetAssetPath(_masterData);
            using var fs = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var stream = new StreamReader(fs);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            using var csv = new CsvReader(stream, config);
            csv.Read();
            csv.ReadHeader();
            csv.Read();
            var database =
                AssetDatabase.LoadAssetAtPath<NameMappingDatabase>(
                    _exportPath + $"/{_databaseName}.asset");
            if (database == null)
            {
                database = CreateInstance<NameMappingDatabase>();
                AssetDatabase.CreateAsset(database, _exportPath + $"/{_databaseName}.asset");
            }

            var databaseSO = new SerializedObject(database);
            databaseSO.Update();
            var mappings = databaseSO.FindProperty("NameMappings");
            mappings.ClearArray();
            while (csv.Read())
            {
                InsertMapping(ref mappings,
                    new NameMapping()
                        { Id = csv.GetField<string>("equipment_id"), Name = csv.GetField<string>("localize_key") });

                databaseSO.ApplyModifiedProperties();
                EditorUtility.SetDirty(database);
                AssetDatabase.SaveAssets();
            }
        }

        private void InsertMapping(ref SerializedProperty mappings, NameMapping mapping)
        {
            mappings.InsertArrayElementAtIndex(mappings.arraySize);
            var element = mappings.GetArrayElementAtIndex(mappings.arraySize - 1);
            element.boxedValue = mapping;
        }

        [ContextMenu("Select export path")]
        public void SelectExportPath()
        {
            _exportPath = EditorUtility.OpenFolderPanel("Select export path", Application.dataPath, "");
            _exportPath = _exportPath.Replace(Application.dataPath, "Assets");
        }
    }
}