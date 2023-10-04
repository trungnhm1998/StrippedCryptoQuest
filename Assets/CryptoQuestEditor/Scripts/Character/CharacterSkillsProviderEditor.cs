using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CryptoQuest.Character;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.Character
{
    [CustomEditor(typeof(CharacterSkillsProvider))]
    public class CharacterSkillsProviderEditor : Editor
    {
        private SerializedProperty _skillMappings;

        private void OnEnable()
        {
            _skillMappings = serializedObject.FindProperty("_skillMappings");
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, serializedObject, this);
            root.Add(CreateOpenCsvButton());
            return root;
        }

        private VisualElement CreateOpenCsvButton()
        {
            var openButton = new Button(ImportCsv)
            {
                text = "Import CSV",
            };
            return openButton;
        }

        private void ImportCsv()
        {
            var path = EditorUtility.OpenFilePanel("Import CSV", "", "csv");
            if (string.IsNullOrEmpty(path)) return;

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var stream = new StreamReader(fs);
                Import(stream);
            }
        }

        private void Import(StreamReader stream)
        {
            using (var csvReader = new CsvReader(stream, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                var mappings = new List<CharacterSkillsProvider.SkillMapping>();
                while (csvReader.Read())
                {
                    var message = csvReader.GetRecord<Foo>();
                    Debug.Log($"{message.SkillId} = {message.Level} + {message.ClassId} + {message.ElementId}");
                    mappings.Add(message.Create());
                }

                _skillMappings.ClearArray(); // only clear after validated
                for (int i = 0; i < mappings.Count; i++)
                {
                    _skillMappings.InsertArrayElementAtIndex(i);
                    var element = _skillMappings.GetArrayElementAtIndex(i);
                    element.boxedValue = mappings[i];
                }

                _skillMappings.serializedObject.ApplyModifiedProperties();
            }
        }

        private class Foo
        {
            [Name("skill_id")] public int SkillId { get; set; }
            [Name("level")] public int Level { get; set; }
            [Name("class_id")] public int ClassId { get; set; }
            [Name("element_ID")] public int ElementId { get; set; }

            public CharacterSkillsProvider.SkillMapping Create()
            {
                return new CharacterSkillsProvider.SkillMapping()
                {
                    SkillId = SkillId,
                    Level = Level,
                    ClassId = ClassId,
                    ElementId = ElementId,
                };
            }
        }
    }
}