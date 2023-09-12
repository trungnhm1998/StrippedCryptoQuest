using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Battle.ScriptableObjects;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Gameplay.Inventory.Currency;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.Tools.ScriptableObjectBrowser;
using NPOI.SS.Formula.Functions;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using AttributeScriptableObject = CryptoQuest.Character.Attributes.AttributeScriptableObject;

namespace CryptoQuestEditor.Gameplay.Gameplay.Monster
{
    public class MonsterDataSOEditor : ScriptableObjectBrowserEditor<EnemyDef>
    {
        private const string DEFAULT_NAME = "Monster";
        private const int ROW_OFFSET = 2;
        private const string ATTRIBUTE_PREFIX = "Default.";
        private const string GOLD_CURRENCY_ASSET_PATH = "Assets/ScriptableObjects/Currency/Gold.asset";

        private const string NORMAL_ATTACK_ABILITY_PATH =
            "Assets/ScriptableObjects/Battle/Skills/Enemy/NormalAttack/EmemyNormalAttack.asset";

        private const string PREFAB_PATH = "Assets/Prefabs/Battle/Enemies/";
        private EnemyDatabase _enemyDatabase;

        public MonsterDataSOEditor()
        {
            CreateDataFolder = false;
            DefaultStoragePath = "Assets/ScriptableObjects/Data/Monster";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);
            _enemyDatabase = GetEnemyDatabase();
            List<EnemyDatabase.Map> enemyMap = new();
            for (int index = ROW_OFFSET; index < allLines.Length; index++)
            {
                // get data form tsv file
                string[] splitedData = allLines[index].Split('\t');
                string name = splitedData[4];
                string replacedName = name.Replace(" ", "");
                string path = DefaultStoragePath + "/" + replacedName + ".asset";
                if (!DataValidator.IsStringsNotNull(splitedData, new List<int>()
                        { 3, 5, 22, 23, 24, 25, 26, 27 }))
                    continue;
                MonsterUnitDataModel dataModel = new MonsterUnitDataModel()
                {
                    MonsterId = int.Parse(splitedData[0]),
                    MonsterName = splitedData[4],
                    ElementId = int.Parse(splitedData[7]),
                    MaxHP = float.Parse(splitedData[8]),
                    HP = float.Parse(splitedData[8]),
                    MP = float.Parse(splitedData[9]),
                    Strength = float.Parse(splitedData[10]),
                    Vitality = float.Parse(splitedData[11]),
                    Agility = float.Parse(splitedData[12]),
                    Intelligence = float.Parse(splitedData[13]),
                    Luck = float.Parse(splitedData[14]),
                    Attack = float.Parse(splitedData[15]),
                    SkillPower = float.Parse(splitedData[16]),
                    Defense = float.Parse(splitedData[17]),
                    EvasionRate = float.Parse(splitedData[18].Replace("%", "")),
                    CriticalRate = float.Parse(splitedData[19].Replace("%", "")),
                    Exp = int.Parse(splitedData[20]),
                    Gold = float.Parse(splitedData[21]),
                    // DropItemID = splitedData[22]
                    DropItemID = "Drop item id",
                    MonsterPrefabName = splitedData[25]
                };
                if (!DataValidator.MonsterDataValidator(dataModel))
                {
                    Debug.Log("Data is not valid");
                    continue;
                }

                EnemyDef instance = null;
                instance = (EnemyDef)AssetDatabase.LoadAssetAtPath(path, typeof(EnemyDef));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<EnemyDef>();
                }

                instance.Editor_SetMonsterPrefab(GetMonsterPrefab(dataModel.MonsterPrefabName));
                List<string> attributeNames = new()
                {
                    "MaxHP", "HP", "MP", "Strength",
                    "Vitality", "Agility", "Intelligence", "Luck", "Attack",
                    "SkillPower", "Defense", "EvasionRate", "CriticalRate"
                };
                AttributeWithValue[] attributeInitValues = InitAttributeValueSetup(dataModel, attributeNames);
                instance.Editor_SetId(dataModel.MonsterId);
                instance.Editor_SetElement(GetElementalSO(dataModel.ElementId));
                instance.Editor_ClearDrop();
                instance.Editor_AddDrop(GetGoldCurrencyRewardInfo(dataModel.Gold));
                instance.Editor_SetEXP(dataModel.Exp);
                instance.Editor_SetStats(attributeInitValues);
                instance.name = replacedName;
               

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
                
                var assetGuid = AssetDatabase.AssetPathToGUID(path);
                instance.SetObjectToAddressableGroup("Enemy");
                EnemyDatabase.Map enemyMapData = new EnemyDatabase.Map()
                {
                    Id = instance.Id,
                    Data = new AssetReferenceT<EnemyDef>(assetGuid)
                };
                
                enemyMap.Add(enemyMapData);
            }
         
            _enemyDatabase.Editor_SetMaps(enemyMap.ToArray());
        }

        private AbilityScriptableObject GetNormalAttackAbility()
        {
            var ability =
                (AbilityScriptableObject)AssetDatabase.LoadAssetAtPath(NORMAL_ATTACK_ABILITY_PATH,
                    typeof(AbilityScriptableObject));
            return ability;
        }

        private GameObject GetMonsterPrefab(string prefabName)
        {
            var path = PREFAB_PATH + prefabName + ".prefab";
            var guid = AssetDatabase.AssetPathToGUID(path);
            return AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
        }

        private CurrencyInfo GetGoldCurrencyRewardInfo(float amount)
        {
            var path = GOLD_CURRENCY_ASSET_PATH;
            var guid = AssetDatabase.AssetPathToGUID(path);
            var goldSo = AssetDatabase.LoadAssetAtPath<CurrencySO>(AssetDatabase.GUIDToAssetPath(guid));
            return new CurrencyInfo(goldSo, amount);
        }

        private Elemental GetElementalSO(int elementId)
        {
            var guids = AssetDatabase.FindAssets("t:Elemental");
            foreach (var guid in guids)
            {
                var asset = AssetDatabase.LoadAssetAtPath<Elemental>(AssetDatabase.GUIDToAssetPath(guid));
                if (asset.Id == elementId)
                {
                    return asset;
                }
            }

            return null;
        }

        private EnemyDatabase GetEnemyDatabase()
        {
            var guids = AssetDatabase.FindAssets("t:EnemyDatabase");

            return AssetDatabase.LoadAssetAtPath<EnemyDatabase>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }


        private AttributeWithValue[] InitAttributeValueSetup(MonsterUnitDataModel dataModel,
            List<string> attributeNames)
        {
            List<AttributeWithValue> values = new();
            foreach (var attributeName in attributeNames)
            {
                AttributeScriptableObject attributeSo = GetAssetsFromType<AttributeScriptableObject>().Where(attribute
                    => attribute.name == ATTRIBUTE_PREFIX + attributeName).First();
                float value = (float)dataModel.GetType().GetProperty(attributeName).GetValue(dataModel, null);
                AttributeWithValue attributeWithValue = new AttributeWithValue();
                attributeWithValue.Attribute = attributeSo;
                attributeWithValue.Value = value;
                values.Add(attributeWithValue);
            }

            return values.ToArray();
        }
    }
}