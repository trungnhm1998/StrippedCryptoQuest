using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Battle.ScriptableObjects;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Loot;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.Tools.ScriptableObjectBrowser;
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
        private const int ID_COLUMN_INDEX = 0;
        private const int LOCALIZE_KEY_COLUMN_INDEX = 1;
        private const int NAME_JP_COLUMN_INDEX = 2;
        private const int DESCRIPTION_JP_COLUMN_INDEX = 3;
        private const int NAME_EN_COLUMN_INDEX = 4;
        private const int DESCRIPTION_EN_COLUMN_INDEX = 5;
        private const int ELEMENT_COLUMN_INDEX = 6;
        private const int ELEMENT_ID_COLUMN_INDEX = 7;
        private const int MAX_HP_COLUMN_INDEX = 8;
        private const int MP_COLUMN_INDEX = 9;
        private const int STRENGTH_COLUMN_INDEX = 10;
        private const int VITALITY_COLUMN_INDEX = 11;
        private const int AGILITY_COLUMN_INDEX = 12;
        private const int INTELLIGENCE_COLUMN_INDEX = 13;
        private const int LUCK_COLUMN_INDEX = 14;
        private const int ATTACK_COLUMN_INDEX = 15;
        private const int MAGIC_ATTACK_COLUMN_INDEX = 16;
        private const int DEFENSE_COLUMN_INDEX = 17;
        private const int EVASION_RATE_COLUMN_INDEX = 18;
        private const int CRITICAL_RATE_COLUMN_INDEX = 19;
        private const int EXP_COLUMN_INDEX = 20;
        private const int GOLD_COLUMN_INDEX = 21;
        private const int SOUL_COLUMN_INDEX = 22;
        private const int DROP_ITEM_ID_COLUMN_INDEX = 23;
        private const int DROP_ITEM_NAME_COLUMN_INDEX = 24;
        private const int DROP_ITEM_RATE_COLUMN_INDEX = 25;
        private const int MONSTER_PREFAB_NAME_COLUMN_INDEX = 26;


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
                if (!DataValidator.IsStringsNotNull(splitedData, 26, new List<int>()
                    {
                        DESCRIPTION_JP_COLUMN_INDEX, DESCRIPTION_EN_COLUMN_INDEX, SOUL_COLUMN_INDEX,
                        DROP_ITEM_ID_COLUMN_INDEX,
                        DROP_ITEM_NAME_COLUMN_INDEX, DROP_ITEM_RATE_COLUMN_INDEX, MONSTER_PREFAB_NAME_COLUMN_INDEX
                    }))
                    continue;
                MonsterUnitDataModel dataModel = new MonsterUnitDataModel()
                {
                    MonsterId = int.Parse(splitedData[ID_COLUMN_INDEX]),
                    MonsterName = splitedData[NAME_EN_COLUMN_INDEX],
                    ElementId = int.Parse(splitedData[ELEMENT_ID_COLUMN_INDEX]),
                    MaxHP = float.Parse(splitedData[MAX_HP_COLUMN_INDEX]),
                    HP = float.Parse(splitedData[MAX_HP_COLUMN_INDEX]),
                    MP = float.Parse(splitedData[MP_COLUMN_INDEX]),
                    Strength = float.Parse(splitedData[STRENGTH_COLUMN_INDEX]),
                    Vitality = float.Parse(splitedData[VITALITY_COLUMN_INDEX]),
                    Agility = float.Parse(splitedData[AGILITY_COLUMN_INDEX]),
                    Intelligence = float.Parse(splitedData[INTELLIGENCE_COLUMN_INDEX]),
                    Luck = float.Parse(splitedData[LUCK_COLUMN_INDEX]),
                    Attack = float.Parse(splitedData[ATTACK_COLUMN_INDEX]),
                    SkillPower = float.Parse(splitedData[MAGIC_ATTACK_COLUMN_INDEX]),
                    Defense = float.Parse(splitedData[DEFENSE_COLUMN_INDEX]),
                    EvasionRate = float.Parse(splitedData[EVASION_RATE_COLUMN_INDEX].Replace("%", "")),
                    CriticalRate = float.Parse(splitedData[CRITICAL_RATE_COLUMN_INDEX].Replace("%", "")),
                    Exp = int.Parse(splitedData[EXP_COLUMN_INDEX]),
                    Gold = float.Parse(splitedData[GOLD_COLUMN_INDEX]),
                    DropItemID = "Drop item id",
                    MonsterPrefabName = splitedData[MONSTER_PREFAB_NAME_COLUMN_INDEX]
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
                instance.Editor_AddDrop(GetExpLoot(dataModel.Exp));
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
            EditorUtility.SetDirty(_enemyDatabase);
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

        private ExpLoot GetExpLoot(float exp)
        {
            return new ExpLoot(exp);
        }

        private CurrencyLootInfo GetGoldCurrencyRewardInfo(float amount)
        {
            var path = GOLD_CURRENCY_ASSET_PATH;
            var guid = AssetDatabase.AssetPathToGUID(path);
            var goldSo = AssetDatabase.LoadAssetAtPath<CurrencySO>(AssetDatabase.GUIDToAssetPath(guid));
            CurrencyInfo currencyInfo = new CurrencyInfo(goldSo, amount);
            return new CurrencyLootInfo(currencyInfo);
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