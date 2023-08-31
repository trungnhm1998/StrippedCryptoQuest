using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.BaseGameplayData;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Encounter;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using AttributeScriptableObject = CryptoQuest.Character.Attributes.AttributeScriptableObject;

namespace CryptoQuestEditor.Gameplay.Gameplay.Monster
{
    public class MonsterDataSOEditor : ScriptableObjectBrowserEditor<MonsterData>
    {
        private const string DEFAULT_NAME = "Monster";
        private const int ROW_OFFSET = 2;
        private const string ATTRIBUTE_PREFIX = "Default.";

        private const string NORMAL_ATTACK_ABILITY_PATH =
            "Assets/ScriptableObjects/Battle/Skills/Enemy/NormalAttack/EmemyNormalAttack.asset";

        private const string PREFAB_PATH = "Assets/Prefabs/Battle/Enemies/";

        public MonsterDataSOEditor()
        {
            this.createDataFolder = false;
            this.defaultStoragePath = "Assets/ScriptableObjects/Data/Monster";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);

            for (int index = ROW_OFFSET; index < allLines.Length; index++)
            {
                // get data form tsv file
                string[] splitedData = allLines[index].Split('\t');
                string name = splitedData[4];
                string replacedName = name.Replace(" ", "");
                string path = this.defaultStoragePath + "/" + replacedName + ".asset";
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
                    Exp = float.Parse(splitedData[20]),
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

                MonsterData instance = null;
                instance = (MonsterData)AssetDatabase.LoadAssetAtPath(path, typeof(MonsterData));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<MonsterData>();
                }

                instance.Editor_SetMonsterPrefab(GetMonsterPrefab(dataModel.MonsterPrefabName));
                List<string> attributeNames = new()
                {
                    "MaxHP", "HP", "MP", "Strength",
                    "Vitality", "Agility", "Intelligence", "Luck", "Attack",
                    "SkillPower", "Defense", "EvasionRate", "CriticalRate"
                };
                AttributeWithValue[] attributeInitValues = InitAttributeValueSetup(dataModel, attributeNames);
                // instance.AttributesToInitialize = attributeInitValues;
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
            }
        }

        private AbilityScriptableObject GetNormalAttackAbility()
        {
            var ability =
                (AbilityScriptableObject)AssetDatabase.LoadAssetAtPath(NORMAL_ATTACK_ABILITY_PATH,
                    typeof(AbilityScriptableObject));
            return ability;
        }

        private AssetReference GetMonsterPrefab(string prefabName)
        {
            var path = PREFAB_PATH + prefabName + ".prefab";
            var guid = AssetDatabase.AssetPathToGUID(path);
            AssetReference monsterPrefab = new(guid);
            return monsterPrefab;
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