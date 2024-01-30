using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CryptoQuest.AbilitySystem;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuestEditor.Gameplay.Gameplay.Abilities
{
    public class MagicImport : ScriptableObject
    {
    }

    public class MagicalAbilitiesSOEditor : ScriptableObjectBrowserEditor<MagicImport>
    {
        private const string DEFAULT_NAME = "";
        private const int ROW_OFFSET = 9;
        private const string NAME_LOCALIZE_TABLE = "AbilityNames";
        private const string DESCRIPTION_LOCALIZE_TABLE = "AbilityDescriptions";

        #region Indexing

        private const int ID_INDEX = 0;
        private const int LOCALIZED_KEY_INDEX = 1;
        private const int ELEMENT_INDEX = 6;
        private const int MP_INDEX = 8;
        private const int EFFECT_TRIGGER_TIMING_INDEX = 9;
        private const int SKILL_TYPE_INDEX = 10;
        private const int CATEGORY_TYPE_INDEX = 11;
        private const int MAIN_EFFECT_TYPE_INDEX = 12;
        private const int MAIN_EFFECT_TARGET_PARAMETER_INDEX = 13;
        private const int TARGET_TYPE_INDEX = 15;
        private const int CONTINUOUS_TURNS_INDEX = 16;
        private const int VALUE_TYPE_INDEX = 17;
        private const int BASE_POWER_INDEX = 18;
        private const int POWER_UPPER_LIMIT_INDEX = 19;
        private const int POWER_LOWER_LIMIT_INDEX = 20;
        private const int SKILL_POWER_THRESHOLD_INDEX = 21;
        private const int POWER_VALUE_ADDED_INDEX = 22;
        private const int POWER_VALUE_REDUCED_INDEX = 23;
        private const int SUCCESS_RATE_INDEX = 36;
        private const int SCENARIO_INDEX = 37;
        private const int VFX_INDEX = 38;
        private const int SUB_EFFECT_MAIN_EFFECT_TYPE_INDEX = 24;
        private const int SUB_EFFECT_MAIN_EFFECT_TARGET_PARAMETER_INDEX = 25;
        private const int SUB_EFFECT_MAIN_TARGET_TYPE_INDEX = 26;
        private const int SUB_EFFECT_CONTINUOUS_TURNS_INDEX = 27;
        private const int SUB_EFFECT_BASE_POWER_INDEX = 28;

        #endregion

        private AbilityAssetMappingEditor _mappingEditor;
        private TagsDef _tagsDef;
        private Dictionary<string, List<TagScriptableObject>> _tagDict = new();

        public MagicalAbilitiesSOEditor()
        {
            CreateDataFolder = false;
            DefaultStoragePath = "Assets/ScriptableObjects/Character/Skills/Castables/Magical";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);
            LoadMappings();
            LoadAbnormalTags();
            LoadTagsDef();
            for (int index = ROW_OFFSET; index < allLines.Length; index++)
            {
                // get data form tsv file
                string[] splitedData = allLines[index].Split('\t');
                if (string.IsNullOrEmpty(splitedData[0])) continue;
                string name = DEFAULT_NAME + splitedData[0];
                string path = DefaultStoragePath + "/" + name + ".asset";

                AbilityDataStruct dataModel = new();

                dataModel.Id = splitedData[ID_INDEX];
                dataModel.LocalizedKey = splitedData[LOCALIZED_KEY_INDEX];
                Debug.Log(splitedData[ID_INDEX]);
                dataModel.ElementId = splitedData[ELEMENT_INDEX];
                dataModel.Mp = int.Parse(splitedData[MP_INDEX]);
                dataModel.EffectTriggerTimingId = splitedData[EFFECT_TRIGGER_TIMING_INDEX];
                dataModel.SkillTypeId = splitedData[SKILL_TYPE_INDEX];
                dataModel.CategoryTypeId = splitedData[CATEGORY_TYPE_INDEX];
                dataModel.MainEffectTypeId = splitedData[MAIN_EFFECT_TYPE_INDEX];
                dataModel.MainEffectTargetParameterId = splitedData[MAIN_EFFECT_TARGET_PARAMETER_INDEX];
                dataModel.TargetTypeId = splitedData[TARGET_TYPE_INDEX];
                dataModel.ContinuousTurns = int.Parse(splitedData[CONTINUOUS_TURNS_INDEX]);
                dataModel.ValueType = splitedData[VALUE_TYPE_INDEX];
                dataModel.BasePower = string.IsNullOrEmpty(splitedData[BASE_POWER_INDEX])
                    ? 0
                    : float.Parse(splitedData[BASE_POWER_INDEX]);
                dataModel.PowerUpperLimit = string.IsNullOrEmpty(splitedData[BASE_POWER_INDEX])
                    ? 0
                    : float.Parse(splitedData[POWER_UPPER_LIMIT_INDEX]);
                dataModel.PowerLowerLimit = string.IsNullOrEmpty(splitedData[BASE_POWER_INDEX])
                    ? 0
                    : float.Parse(splitedData[POWER_LOWER_LIMIT_INDEX]);
                dataModel.SkillPowerThreshold = string.IsNullOrEmpty(splitedData[BASE_POWER_INDEX])
                    ? 0
                    : float.Parse(splitedData[SKILL_POWER_THRESHOLD_INDEX]);
                dataModel.PowerValueAdded = string.IsNullOrEmpty(splitedData[BASE_POWER_INDEX])
                    ? 0
                    : float.Parse(splitedData[POWER_VALUE_ADDED_INDEX]);
                dataModel.PowerValueReduced = string.IsNullOrEmpty(splitedData[BASE_POWER_INDEX])
                    ? 0
                    : float.Parse(splitedData[POWER_VALUE_REDUCED_INDEX]);
                dataModel.SuccessRate = string.IsNullOrEmpty(splitedData[BASE_POWER_INDEX])
                    ? 0
                    : float.Parse(splitedData[SUCCESS_RATE_INDEX]);
                dataModel.ScenarioId = splitedData[SCENARIO_INDEX];
                dataModel.VfxId = splitedData[VFX_INDEX];

                if (!string.IsNullOrEmpty(splitedData[SUB_EFFECT_MAIN_EFFECT_TYPE_INDEX]))
                {
                    SubEffectDataStruct subData = new()
                    {
                        EffectTypeId = splitedData[SUB_EFFECT_MAIN_EFFECT_TYPE_INDEX],
                        EffectTargetParameterId = splitedData[SUB_EFFECT_MAIN_EFFECT_TARGET_PARAMETER_INDEX],
                        TargetTypeId = splitedData[SUB_EFFECT_MAIN_TARGET_TYPE_INDEX],
                        ContinuousTurns = int.Parse(splitedData[SUB_EFFECT_CONTINUOUS_TURNS_INDEX]),
                        BasePower = float.Parse(splitedData[SUB_EFFECT_BASE_POWER_INDEX]),
                    };
                    dataModel.SubEffectData = subData;
                    if (subData.EffectTypeId == "99") continue;
                }

                if (dataModel.MainEffectTypeId == "99") continue;
                if (dataModel.IsSubEffectValid()) continue;

                CastEffectsOnTargetAbility instance = null;
                instance = CreateInstance(instance, dataModel, path);
                SetInstanceProperty(instance, dataModel);
                SetEffects(instance, dataModel);
                SetLocalized(instance, dataModel);
                SetIgnoreTag(instance, dataModel);

                instance.name = name;

                if (!AssetDatabase.Contains(instance))
                {
                    AssetDatabase.CreateAsset(instance, path);
                    AssetDatabase.SaveAssets();
                    callback(instance);
                }
                else
                {
                    EditorUtility.SetDirty(instance);
                }
            }
        }

        private void SetEffects(CastEffectsOnTargetAbility instance, AbilityDataStruct data)
        {
            var serializedObject = new SerializedObject(instance);
            var property = serializedObject.FindProperty("_effects");
            property.ClearArray();
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            if (data.MainEffectTypeId == "5")
                SetRemoveAbnormalInstance(serializedObject, data);

            var mainEffect = GetEffect(data.MainEffectTypeId, data.MainEffectTargetParameterId);
            GameplayEffectDefinition[] effects = new GameplayEffectDefinition[2];
            effects[0] = mainEffect;
            if (effects[0] == null) return;
            property.InsertArrayElementAtIndex(0);
            property.GetArrayElementAtIndex(0).objectReferenceValue = effects[0];

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private CastEffectsOnTargetAbility CreateInstance(CastEffectsOnTargetAbility instance,
            AbilityDataStruct data, string path)
        {
            if (data.MainEffectTypeId != "5")
            {
                instance = (CastEffectsOnTargetAbility)AssetDatabase.LoadAssetAtPath(path,
                    typeof(CastEffectsOnTargetAbility));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<CastEffectsOnTargetAbility>();
                }
            }
            else
            {
                instance = (CastRemoveAbnormalAbility)AssetDatabase.LoadAssetAtPath(path,
                    typeof(CastRemoveAbnormalAbility));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<CastRemoveAbnormalAbility>();
                }
            }

            return instance;
        }

        private void SetInstanceProperty(CastEffectsOnTargetAbility instance, AbilityDataStruct data)
        {
            SkillInfo skillInfo = new SkillInfo();
            skillInfo.Id = int.Parse(data.Id);
            skillInfo.SkillParameters = new SkillParameters();
            skillInfo.SkillType = ESkillType.Magic;
            skillInfo.SkillParameters.Element = GetElement(data.ElementId);
            skillInfo.SkillParameters.BasePower = data.BasePower;
            skillInfo.SkillParameters.PowerUpperLimit = data.PowerUpperLimit;
            skillInfo.SkillParameters.PowerLowerLimit = data.PowerLowerLimit;
            skillInfo.SkillParameters.SkillPowerThreshold = data.SkillPowerThreshold;
            skillInfo.SkillParameters.PowerValueAdded = data.PowerValueAdded;
            skillInfo.SkillParameters.PowerValueReduced = data.PowerValueReduced;
            skillInfo.SkillParameters.ContinuesTurn = data.ContinuousTurns;
            skillInfo.SkillParameters.EffectType = GetEffectType(data.MainEffectTypeId);
            skillInfo.SkillParameters.IsFixed = data.ValueType == "1" ? true : false;
            skillInfo.Cost = data.Mp;
            skillInfo.UsageScenarioSO = data.ScenarioId == "1"
                ? EAbilityUsageScenario.Field
                : data.ScenarioId == "2"
                    ? EAbilityUsageScenario.Battle
                    : data.ScenarioId == "4"
                        ? EAbilityUsageScenario.World
                        : EAbilityUsageScenario.Field | EAbilityUsageScenario.Battle;

            skillInfo.VfxId = string.IsNullOrEmpty(data.VfxId) ? 0 : int.Parse(data.VfxId);
            CustomExecutionAttributeCaptureDef attributeCaptureDef = new();
            attributeCaptureDef.Attribute = GetAttribute(data.MainEffectTargetParameterId);
            attributeCaptureDef.CaptureFrom = EGameplayEffectCaptureSource.Target;
            skillInfo.SkillParameters.TargetAttribute = attributeCaptureDef;

            GameplayEffectContext ctx = new GameplayEffectContext(skillInfo);


            var serializedObject = new SerializedObject(instance);
            var property = serializedObject.FindProperty("_context");
            var successRate = serializedObject.FindProperty("<SuccessRate>k__BackingField");
            var targetType = serializedObject.FindProperty("<TargetType>k__BackingField");
            property.boxedValue = ctx;
            successRate.floatValue = data.SuccessRate;
            targetType.objectReferenceValue = GetSkillTargetType(data.TargetTypeId);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private void SetLocalized(CastEffectsOnTargetAbility instance, AbilityDataStruct data)
        {
            LocalizedString skillName = new(NAME_LOCALIZE_TABLE, data.LocalizedKey);
            LocalizedString skillDescription = new(DESCRIPTION_LOCALIZE_TABLE, data.LocalizedKey);
            instance.SetSkillName(skillName);
            instance.SetSkillDescription(skillDescription);
        }

        private void SetIgnoreTag(CastEffectsOnTargetAbility instance, AbilityDataStruct data)
        {
            string targetType = data.TargetTypeId;
            string[] allyTargetTypeIds = new[] { "1", "2", "3" };
            var serializedObject = new SerializedObject(instance);
            var property = serializedObject.FindProperty("tags.TargetTags.IgnoreTags");
            if (property == null) return;
            property.ClearArray();
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            if (!allyTargetTypeIds.Contains(targetType)) return;

            property.InsertArrayElementAtIndex(0);
            property.GetArrayElementAtIndex(0).objectReferenceValue = _tagsDef.DeadTag;

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private void LoadAbnormalTags()
        {
            var maps = _mappingEditor.MagicalEffectMaps[3].GameEffectMaps;

            foreach (var map in maps)
            {
                _tagDict.TryAdd(map.Id, new List<TagScriptableObject>(map.Value.GrantedTags.ToList()));
            }
        }

        private void SetRemoveAbnormalInstance(SerializedObject serializedObject, AbilityDataStruct data)
        {
            var property = serializedObject.FindProperty("<CancelEffectWithTags>k__BackingField");
            if (property == null) return;

            property.ClearArray();
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            var tagList = _tagDict[data.MainEffectTargetParameterId];
            for (int i = 0; i < tagList.Count; i++)
            {
                property.InsertArrayElementAtIndex(i);
                property.GetArrayElementAtIndex(i).objectReferenceValue = tagList[i];
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private Elemental GetElement(string id)
        {
            foreach (var map in _mappingEditor.ElementMaps)
            {
                if (map.Id == id)
                {
                    return map.Value;
                }
            }

            return null;
        }

        private GameplayEffectDefinition GetEffect(string effectTypeId, string targetParam)
        {
            foreach (var map in _mappingEditor.MagicalEffectMaps)
            {
                if (map.Id == effectTypeId)
                {
                    if (effectTypeId == "1")
                        return map.GameEffectMaps[0].Value;
                    foreach (var effectMap in map.GameEffectMaps)
                    {
                        if (effectMap.Id == targetParam)
                            return effectMap.Value;
                    }
                }
            }

            return null;
        }

        private AttributeScriptableObject GetAttribute(string id)
        {
            foreach (var map in _mappingEditor.TargetParameterMaps)
            {
                if (map.Id == id)
                {
                    return map.Value;
                }
            }

            return null;
        }

        private EEffectType GetEffectType(string id)
        {
            foreach (var map in _mappingEditor.EffectTypeMaps)
            {
                if (map.Id == id)
                {
                    return map.Value;
                }
            }

            return default;
        }

        private SkillTargetType GetSkillTargetType(string id)
        {
            foreach (var map in _mappingEditor.TargetTypeMaps)
            {
                if (map.Id == id)
                {
                    return map.Value;
                }
            }

            return null;
        }

        public GameplayEffectDefinition GetGameplayEffectDefinition(string id)
        {
            foreach (var map in _mappingEditor.GameEffectMaps)
            {
                if (map.Id == id)
                {
                    return map.Value;
                }
            }

            return null;
        }

        private void LoadMappings()
        {
            var guid = AssetDatabase.FindAssets("t:AbilityAssetMappingEditor");
            _mappingEditor =
                AssetDatabase.LoadAssetAtPath<AbilityAssetMappingEditor>(AssetDatabase.GUIDToAssetPath(guid[0]));
        }

        private void LoadTagsDef()
        {
            var guid = AssetDatabase.FindAssets("t:TagsDef");
            _tagsDef =
                AssetDatabase.LoadAssetAtPath<TagsDef>(AssetDatabase.GUIDToAssetPath(guid[0]));
        }
    }
}