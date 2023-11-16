using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CryptoQuest.Character;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.Character
{
    public class HeroSkillSetData
    {
        [Name("class_id")] public int ClassId { get; set; }
        [Name("skill_id")] public int SkillId { get; set; }
        [Name("element_ID")] public int ElementId { get; set; }
        [Name("level")] public int Level { get; set; }
    }

    [CustomEditor(typeof(HeroSkillSetSO), true)]
    public class HeroSkillSetSOEditor : Editor
    {
        private HeroSkillSetSO Target => (HeroSkillSetSO)target;
        private SerializedProperty _skillMappings;

        protected virtual void OnEnable()
        {
            _skillMappings = serializedObject.FindProperty("_skillMappings");
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
                var rows = new List<HeroSkillSetData>();
                while (csvReader.Read())
                {
                    EditorUtility.DisplayProgressBar("Reading CSV", "Reading each row",
                        csvReader.Context.Row / (float)csvReader.Context.Record.Length);
                    if (IgnoreRow(csvReader.Context)) continue;
                    var message = csvReader.GetRecord<HeroSkillSetData>();
                    rows.Add(message);
                }

                if (rows.Count == 0) return;
                _skillMappings.ClearArray(); // only clear after validated
                for (int i = 0; i < rows.Count; i++)
                {
                    _skillMappings.InsertArrayElementAtIndex(i);
                    var element = _skillMappings.GetArrayElementAtIndex(i);
                    var skillSet = rows[i];
                    element.boxedValue = new HeroSkillsSet()
                    {
                        Class = skillSet.ClassId,
                        Skill = skillSet.SkillId,
                        Element = skillSet.ElementId,
                        Level = skillSet.Level
                    };
                }

                _skillMappings.serializedObject.ApplyModifiedProperties();
                EditorUtility.ClearProgressBar();
            }
        }

        protected virtual bool IgnoreRow(ReadingContext contextRawRecord)
        {
            return contextRawRecord.Row == 1;
        }
    }
}