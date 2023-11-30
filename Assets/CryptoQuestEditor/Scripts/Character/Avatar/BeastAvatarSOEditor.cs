using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CryptoQuest.Beast.Avatar;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace CryptoQuestEditor.CryptoQuestEditor.Scripts.Character.Avatar
{
    public class AvatarBeastSetData
    {
        [Name("beast_id")] public int BeastId { get; set; }
        [Name("class_id")] public int ClassId { get; set; }
        [Name("element_id")] public int ElementId { get; set; }
        [Name("image_name")] public string ImageName { get; set; }
    }

    [CustomEditor(typeof(BeastAvatarSO), true)]
    public class BeastAvatarSOEditor : Editor
    {
        private BeastAvatarSO Target => (BeastAvatarSO)target;
        private SerializedProperty _avatarMappings;

        private const string BEAST_AVATAR_PATH = "Assets/Arts/Beasts/";

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
                var rows = new List<AvatarBeastSetData>();
                while (csvReader.Read())
                {
                    EditorUtility.DisplayProgressBar("Reading CSV", "Reading each row",
                        csvReader.Context.Row / (float)csvReader.Context.Record.Length);
                    if (IgnoreRow(csvReader.Context)) continue;
                    var message = csvReader.GetRecord<AvatarBeastSetData>();

                    if (message.BeastId == 0 || message.ClassId == 0 || message.ElementId == 0) continue;

                    if (rows.Exists(x =>
                            x.BeastId == message.BeastId &&
                            x.ClassId == message.ClassId &&
                            x.ElementId == message.ElementId)) continue;

                    rows.Add(message);
                }

                if (rows.Count == 0) return;
                _avatarMappings.ClearArray(); // only clear after validated
                for (int i = 0; i < rows.Count; i++)
                {
                    _avatarMappings.InsertArrayElementAtIndex(i);
                    var element = _avatarMappings.GetArrayElementAtIndex(i);
                    var avatarSet = rows[i];

                    var guids = AssetDatabase.FindAssets("t:sprite", new[] { BEAST_AVATAR_PATH });

                    foreach (var guid in guids)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(guid);
                        var asset = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                        var assetRef = new AssetReferenceT<Sprite>(guid);
                        assetRef.SetEditorAsset(asset);

                        GetAvatarId(asset, rows);
                    }

                    // TODO: Check and set image here

                    // element.boxedValue = new BeastAvatarData()
                    // {
                    //     BeastId = avatarSet.BeastId,
                    //     ClassId = avatarSet.ClassId,
                    //     ElementId = avatarSet.ElementId,
                    //     Image = assetRef,
                    // };
                }

                _avatarMappings.serializedObject.ApplyModifiedProperties();
                EditorUtility.ClearProgressBar();
            }
        }

        private void GetAvatarId(Sprite asset, List<AvatarBeastSetData> element) { }

        protected virtual bool IgnoreRow(ReadingContext contextRawRecord)
        {
            return contextRawRecord.Row == 1;
        }
    }
}