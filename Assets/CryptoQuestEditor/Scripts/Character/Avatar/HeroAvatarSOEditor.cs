using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CryptoQuest.Character.Avatar;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.Character.Avatar
{
    public class AvatarHeroSetData
    {
        [Name("character_id")] public int CharacterId { get; set; }
        [Name("class_id")] public int ClassId { get; set; }
        [Name("image_name")] public string ImageName { get; set; }
    }

    [CustomEditor(typeof(HeroAvatarSetSO), true)]
    public class HeroAvatarSOEditor : Editor
    {
        private HeroAvatarSetSO Target => (HeroAvatarSetSO)target;
        private SerializedProperty _avatarMappings;

        protected virtual void OnEnable()
        {
            _avatarMappings = serializedObject.FindProperty("_avatarMappings");
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            // add Load all data button
            root.Add(new Button(() =>
            {
                ImportCsv();
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            })
            {
                text = "Import CSV",
            });
            return root;
        }

        private void ImportCsv()
        {
            var path = EditorUtility.OpenFilePanel("Import CSV", "", "csv");
            if (string.IsNullOrEmpty(path)) return;

            EditorUtility.DisplayProgressBar("Importing CSV", "Importing CSV", 0);

            try
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var stream = new StreamReader(fs);
                    ReadStream(stream);
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

        private void ReadStream(StreamReader stream)
        {
            using (var csvReader = new CsvReader(stream, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                var rows = new List<AvatarHeroSetData>();
                while (csvReader.Read())
                {
                    EditorUtility.DisplayProgressBar("Reading CSV", "Reading each row",
                        csvReader.Context.Row / (float)csvReader.Context.Record.Length);
                    if (IgnoreRow(csvReader.Context)) continue;
                    var message = csvReader.GetRecord<AvatarHeroSetData>();
                    rows.Add(message);
                }

                if (rows.Count == 0) return;
                _avatarMappings.ClearArray(); // only clear after validated
                for (int i = 0; i < rows.Count; i++)
                {
                    _avatarMappings.InsertArrayElementAtIndex(i);
                    var element = _avatarMappings.GetArrayElementAtIndex(i);
                    var avatarSet = rows[i];
                    element.boxedValue = new HeroAvatarSet()
                    {
                        CharacterId = avatarSet.CharacterId,
                        ClassId = avatarSet.ClassId,
                        ImageName = avatarSet.ImageName
                    };
                }

                _avatarMappings.serializedObject.ApplyModifiedProperties();
                EditorUtility.ClearProgressBar();
            }
        }

        protected virtual bool IgnoreRow(ReadingContext contextRawRecord)
        {
            return contextRawRecord.Row == 1;
        }
    }
}