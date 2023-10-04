using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using IndiGames.Core.Database;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace IndiGamesEditor.Core.Database
{
    public abstract class DatabaseImporterEditor<TDatabase, TKey, TAsset, TPoco> : AssetReferenceDatabaseEditor
        where TDatabase : AssetReferenceDatabaseT<TKey, TAsset>
        where TAsset : ScriptableObject
    {
        private SerializedProperty _maps;
        protected virtual string ExportPath { get; set; }

        protected override void OnEnable()
        {
            base.OnEnable();
            _maps = serializedObject.FindProperty("_maps");
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            root.Add(base.CreateInspectorGUI());
            var importButton = new Button(ImportCsv)
            {
                text = "Import CSV",
            };
            root.Add(importButton);
            return root;
        }

        private void ImportCsv()
        {
            var path = EditorUtility.OpenFilePanel("Import CSV", "", "csv");
            if (string.IsNullOrEmpty(path)) return;

            if (HasExportPath() == false) return;

            EditorUtility.DisplayProgressBar("Importing CSV", "Importing CSV", 0);

            try
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var stream = new StreamReader(fs);
                    Import(stream);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                EditorUtility.ClearProgressBar();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private bool HasExportPath()
        {
            if (!string.IsNullOrEmpty(ExportPath)) return true;
            ExportPath = EditorUtility.OpenFolderPanel("Select folder to export asset", "", "");
            return string.IsNullOrEmpty(ExportPath) == false;
        }

        private void Import(StreamReader stream)
        {
            using (var csvReader = new CsvReader(stream, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                var rows = new List<TPoco>();
                while (csvReader.Read())
                {
                    EditorUtility.DisplayProgressBar("Reading CSV", "Reading each row",
                        csvReader.Context.Row / (float)csvReader.Context.Record.Length);
                    if (IgnoreRow(csvReader.Context)) continue;
                    var message = csvReader.GetRecord<TPoco>();
                    rows.Add(message);
                }

                if (rows.Count == 0) return;
                _maps.ClearArray(); // only clear after validated
                for (int i = 0; i < rows.Count; i++)
                {
                    _maps.InsertArrayElementAtIndex(i);
                    var element = _maps.GetArrayElementAtIndex(i);
                    element.boxedValue = CreateAssetMap(rows, i);
                }

                _maps.serializedObject.ApplyModifiedProperties();
                EditorUtility.ClearProgressBar();
            }
        }

        protected virtual bool IgnoreRow(ReadingContext contextRawRecord)
        {
            return false;
        }

        protected virtual AssetReferenceDatabaseT<TKey, TAsset>.Map CreateAssetMap(List<TPoco> sheet, int i)
        {
            var data = sheet[i];
            // remove every thing until Assets
            var finalExportPath = ExportPath.Substring(ExportPath.IndexOf("Assets"));
            var assetName = GetAssetName(data);
            var path = $"{finalExportPath}/{assetName}.asset";
            EditorUtility.DisplayProgressBar($"Exporting asset into:\n{path}", $"Create {assetName}",
                i / (float)sheet.Count);
            var asset = AssetDatabase.LoadAssetAtPath<TAsset>(path);
            if (asset == null || !AssetDatabase.Contains(asset))
            {
                asset = CreateInstance<TAsset>();
                AssetDatabase.CreateAsset(asset, path);
            }

            ModifyAndSaveAsset(data, asset);
            var guid = AssetDatabase.AssetPathToGUID(path);
            return new AssetReferenceDatabaseT<TKey, TAsset>.Map()
            {
                Id = GetId(data),
                Data = new AssetReferenceT<TAsset>(guid),
            };
        }

        private void ModifyAndSaveAsset(TPoco data, TAsset asset)
        {
            var so = new SerializedObject(asset);
            OnAssetModified(ref so, asset, data);
            so.ApplyModifiedPropertiesWithoutUndo();
            so.Update();
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.SaveAssets();
        }

        protected abstract TKey GetId(TPoco data);

        /// <summary>
        /// Use this to modify the SO with the data from the CSV
        /// </summary>
        protected abstract void OnAssetModified(ref SerializedObject serializeObjectInstance, TAsset asset, TPoco data);

        protected abstract string GetAssetName(TPoco data);
    }
}