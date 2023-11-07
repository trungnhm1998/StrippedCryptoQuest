using System;
using System.IO;
using CryptoQuest.AbilitySystem.Abilities.PostNormalAttackPassive;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Gameplay.Abilities
{
    public class TagSOEditor : ScriptableObjectBrowserEditor<TagScriptableObject>
    {
        private const int ROW_OFFSET = 0;
        private TagScriptableObject _buffTag;
        private TagScriptableObject _debuffTag;
        private AbilityAssetMappingEditor _mappingEditor;

        public TagSOEditor()
        {
            CreateDataFolder = false;
            DefaultStoragePath = "Assets/ScriptableObjects/Battle/Tags/";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            LoadTags();
            string[] allLines = File.ReadAllLines(directory);
            for (int index = ROW_OFFSET; index < allLines.Length; index++)
            {
                // get data form tsv file
                string[] splitedData = allLines[index].Split('\t');
                if (string.IsNullOrEmpty(splitedData[0])) continue;
                CreateBuff(splitedData, true, callback);
                CreateBuff(splitedData, false, callback);
            }
        }

        public void CreateBuff(string[] data, bool isBuff, Action<ScriptableObject> callback)
        {
            string type = isBuff ? "Up" : "Down";
            string name = data[1] + type;
            string folder = isBuff ? "Buffs" : "Debuffs";
            string storagePath = DefaultStoragePath + folder;
            string path = storagePath + "/" + name + ".asset";
            TagScriptableObject instanceBuff = null;
            instanceBuff = (TagScriptableObject)AssetDatabase.LoadAssetAtPath(path, typeof(TagScriptableObject));
            if (instanceBuff == null || !AssetDatabase.Contains(instanceBuff))
            {
                instanceBuff = ScriptableObject.CreateInstance<TagScriptableObject>();
            }

            instanceBuff.name = name;
            var serializedObject = new SerializedObject(instanceBuff);
            var property = serializedObject.FindProperty("_parent");
            property.objectReferenceValue = isBuff ? _buffTag : _debuffTag;
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            if (!AssetDatabase.Contains(instanceBuff))
            {
                AssetDatabase.CreateAsset(instanceBuff, path);
                AssetDatabase.SaveAssets();
                callback(instanceBuff);
            }
            else
            {
                EditorUtility.SetDirty(instanceBuff);
            }
        }

        private void LoadTags()
        {
            _buffTag = AssetDatabase.LoadAssetAtPath<TagScriptableObject>(
                "Assets/ScriptableObjects/Battle/Tags/Abnormals/Buff.asset");
            _debuffTag =
                AssetDatabase.LoadAssetAtPath<TagScriptableObject>(
                    "Assets/ScriptableObjects/Battle/Tags/Abnormals/Debuff.asset");
        }
    }
}