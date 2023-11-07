using System;
using System.Collections.Generic;
using System.IO;
using CryptoQuest.AbilitySystem.EffectActions;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Gameplay.Abilities
{
    public class EditorEffects : ScriptableObject { }

    public class GameEffectSOEditor : ScriptableObjectBrowserEditor<EditorEffects>
    {
        private const int ROW_OFFSET = 0;
        private List<TagScriptableObject> _tags = new();
        private EffectExecutionCalculationBase _buffCal;
        private EffectExecutionCalculationBase _debuffCal;
        private EffectExecutionCalculationBase _baseMagicCal;
        private AbilityAssetMappingEditor _mappingEditor;

        public GameEffectSOEditor()
        {
            CreateDataFolder = false;
            DefaultStoragePath = "Assets/ScriptableObjects/Character/Skills/Effects";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            LoadMappings();
            LoadTags();
            LoadCalculations();
            string[] allLines = File.ReadAllLines(directory);
            for (int index = ROW_OFFSET; index < allLines.Length; index++)
            {
                // get data form tsv file
                string[] splitedData = allLines[index].Split('\t');
                if (string.IsNullOrEmpty(splitedData[0])) continue;
                CreateEffect(splitedData, true, callback);
                CreateEffect(splitedData, false, callback);
            }
        }

        public void CreateEffect(string[] data, bool isBuff, Action<ScriptableObject> callback)
        {
            string type = isBuff ? "Up" : "Down";
            string name = data[1] + type;
            string effectName = "GE_" + name;
            string folder = isBuff ? "/GE_Buffs" : "/GE_Debuffs";
            string storagePath = DefaultStoragePath + folder;
            string path = storagePath + "/" + effectName + ".asset";
            GameplayEffectDefinition instanceBuff = null;
            instanceBuff =
                (GameplayEffectDefinition)AssetDatabase.LoadAssetAtPath(path, typeof(GameplayEffectDefinition));
            if (instanceBuff == null || !AssetDatabase.Contains(instanceBuff))
            {
                instanceBuff = ScriptableObject.CreateInstance<GameplayEffectDefinition>();
            }

            instanceBuff.name = effectName;
            TagScriptableObject tag = _tags.Find(x => x.name == name);
            var serializedObject = new SerializedObject(instanceBuff);
            var property = serializedObject.FindProperty("<GrantedTags>k__BackingField");
            var propertyPolicy = serializedObject.FindProperty("_policy");
            var propertyEffectDetails = serializedObject.FindProperty("<EffectDetails>k__BackingField");
            var propertyCalculations = serializedObject.FindProperty("<ExecutionCalculations>k__BackingField");
            property.InsertArrayElementAtIndex(0);
            property.GetArrayElementAtIndex(0).objectReferenceValue = tag;
            propertyPolicy.boxedValue = new TurnBasePolicy();
            EffectDetails effectDetails = new EffectDetails();
            effectDetails.StackingType = EModifierType.External;
            propertyEffectDetails.boxedValue = effectDetails;
            EffectExecutionCalculationBase[] calculations = new EffectExecutionCalculationBase[2];
            calculations[0] = _baseMagicCal;
            calculations[1] = isBuff ? _buffCal : _debuffCal;
            propertyCalculations.InsertArrayElementAtIndex(0);
            propertyCalculations.GetArrayElementAtIndex(0).objectReferenceValue = calculations[0];
            propertyCalculations.InsertArrayElementAtIndex(1);
            propertyCalculations.GetArrayElementAtIndex(1).objectReferenceValue = calculations[1];
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            if (!AssetDatabase.Contains(instanceBuff))
            {
                AssetDatabase.CreateAsset(instanceBuff, path);
                AssetDatabase.SaveAssets();
                callback(instanceBuff);
                for (var index = 0; index < _mappingEditor.EffectPairMaps.Count; index++)
                {
                    var map = _mappingEditor.EffectPairMaps[index];
                    if (map.AttributeId == data[0])
                    {
                        if (!map.GameplayEffectDefinitions.Contains(instanceBuff))
                            map.GameplayEffectDefinitions.Add(instanceBuff);
                        return;
                    }
                }

                GameEffectPairMap pairMap = new();
                pairMap.AttributeId = data[0];
                pairMap.GameplayEffectDefinitions.Add(instanceBuff);
                _mappingEditor.EffectPairMaps.Add(pairMap);
                EditorUtility.SetDirty(_mappingEditor);
            }
            else
            {
                EditorUtility.SetDirty(instanceBuff);
            }
        }

        private void LoadTags()
        {
            string[] guids = AssetDatabase.FindAssets("t:TagScriptableObject");
            foreach (var guid in guids)
            {
                TagScriptableObject obj =
                    AssetDatabase.LoadAssetAtPath<TagScriptableObject>(AssetDatabase.GUIDToAssetPath(guid));
                if (obj == null) continue;
                _tags.Add(obj);
            }
        }

        private void LoadCalculations()
        {
            string[] guids = AssetDatabase.FindAssets("t:EffectExecutionCalculationBase");
            foreach (var guid in guids)
            {
                EffectExecutionCalculationBase obj =
                    AssetDatabase.LoadAssetAtPath<EffectExecutionCalculationBase>(AssetDatabase.GUIDToAssetPath(guid));
                if (obj.name == "BaseMagicPowerCalculation") _baseMagicCal = obj;
                if (obj.name == "BuffCalculation") _buffCal = obj;
                if (obj.name == "DebuffCalculation") _debuffCal = obj;
            }
        }

        private void LoadMappings()
        {
            var guid = AssetDatabase.FindAssets("t:AbilityAssetMappingEditor");
            _mappingEditor =
                AssetDatabase.LoadAssetAtPath<AbilityAssetMappingEditor>(AssetDatabase.GUIDToAssetPath(guid[0]));
        }
    }
}