using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.CSV;
using UnityEngine;

namespace CryptoQuestEditor.Helper
{
    public class CsvImportExportLocalization : Editor
    {
        [MenuItem("Tools/Localization/Export All CSV Files")]
        public static void PullAllExtensions()
        {
            // Get every String Table Collection
            string folderPath = EditorUtility.OpenFolderPanel("Select Folder", "", "");
            var stringTableCollections = LocalizationEditorSettings.GetStringTableCollections();

            foreach (var collection in stringTableCollections)
            {
                // Its possible a String Table Collection may have more than one extension.
                foreach (var extension in collection.Extensions)
                {
                    if (extension is CsvExtension csvExtension)
                    {
                        var filePath = GetFilePath(folderPath, collection.name);
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            using (var stream = new StreamWriter(filePath, false, Encoding.UTF8))
                            {
                                Csv.Export(stream, collection, csvExtension.Columns);
                                Debug.Log($"Exported {collection.name}!");
                            }
                        }
                    }
                }
            }
        }

        [MenuItem("Tools/Localization/Import Single CSV File")]
        public static void ImportSingleExtensions()
        {
            // Get every String Table Collection
            string filePath = EditorUtility.OpenFilePanel("Select CSV file", "", "csv");
            var stringTableCollections = LocalizationEditorSettings.GetStringTableCollections();

            using (var stream = new StreamReader(filePath))
            {
                
                foreach (var collection in stringTableCollections)
                {
                    // Its possible a String Table Collection may have more than one extension.
                    foreach (var extension in collection.Extensions)
                    {
                        if (extension is CsvExtension csvExtension)
                        {
                            if (Path.GetFileNameWithoutExtension(filePath) != collection.name) continue;
                            Csv.ImportInto(stream, collection, csvExtension.Columns);
                            Debug.Log($"Imported {collection.name}!");
                        }
                    }
                }
            }
        }

        [MenuItem("Tools/Localization/Import All CSV Files")]
        public static void ImportAllExtensions()
        {
            // Get every String Table Collection
            string folderPath = EditorUtility.OpenFolderPanel("Select Folder", "", "");
            var stringTableCollections = LocalizationEditorSettings.GetStringTableCollections();

            foreach (var collection in stringTableCollections)
            {
                // Its possible a String Table Collection may have more than one extension.
                foreach (var extension in collection.Extensions)
                {
                    if (extension is CsvExtension csvExtension)
                    {
                        var filePath = GetFilePath(folderPath, collection.name);
                        if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                        {
                            using (var stream = new StreamReader(filePath))
                            {
                                Csv.ImportInto(stream, collection, csvExtension.Columns);
                                Debug.Log($"Imported {collection.name}!");
                            }
                        }
                    }
                }
            }
        }

        private static string GetFilePath(string folderPath, string collectionName)
        {
            if (string.IsNullOrEmpty(folderPath)) return "";

            return Path.Combine(folderPath, $"{collectionName}.csv");
        }
    }
}
