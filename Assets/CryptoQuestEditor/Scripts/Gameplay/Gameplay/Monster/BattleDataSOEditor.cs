using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CryptoQuest.Gameplay.BaseGameplayData;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Encounter;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Gameplay.Monster
{
    public class BattleDataSOEditor : ScriptableObjectBrowserEditor<EncounterGroups>
    {
        private const string DEFAULT_NAME = "MonsterParty_";
        private const int ROW_OFFSET = 2;

        public BattleDataSOEditor()
        {
            this.createDataFolder = false;
            this.defaultStoragePath = "Assets/ScriptableObjects/Data/MonsterParty";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);

            for (int index = ROW_OFFSET; index < allLines.Length; index++)
            {
                // get data form tsv file
                string[] splitedData = allLines[index].Split('\t');
                string name = DEFAULT_NAME + splitedData[0];
                string path = this.defaultStoragePath + "/" + name + ".asset";
                MonsterPartyDataModel dataModel = new MonsterPartyDataModel()
                {
                    MonserPartyId = int.Parse(splitedData[0]),
                    MonsterGroupingProperty = splitedData[2]
                };
                if (!DataValidator.MonsterPartyValidator(dataModel))
                {
                    Debug.Log("Data is not valid");
                    continue;
                }

                EncounterGroups instance = null;
                instance = (EncounterGroups)AssetDatabase.LoadAssetAtPath(path, typeof(EncounterGroups));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<EncounterGroups>();
                }

                List<string> groups = new()
                {
                    splitedData[2], splitedData[3], splitedData[4], splitedData[5]
                };
                if (!DataValidator.IsValidNumberOfMonsterSetup(groups)) continue;
                instance.Editor_SetEnemyGroups(ConfigMonsterGroup(groups));
                instance.Editor_SetId(dataModel.MonserPartyId);
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

        private EncounterGroups.CharacterGroup ConfigMonsterDataSOProperties(string groupProperties)
        {
            string[] splitArray = groupProperties.Split(",", StringSplitOptions.None);
            List<CharacterDataSO> monsterDataGroup = new();
            foreach (var id in splitArray)
            {
                if (string.IsNullOrEmpty(id)) continue;
                var assets = GetAssetsFromType<MonsterDataSO>().Where(monster
                    => monster.MonsterId == int.Parse(id));
                MonsterDataSO monsterDataSo = assets.Count() > 0 ? assets.First() : null;
                if (monsterDataSo != null)
                {
                    monsterDataGroup.Add(monsterDataSo);
                }
            }

            EncounterGroups.CharacterGroup characterGroup = new();
            characterGroup.Editor_SetCharacters(monsterDataGroup.ToArray());
            return characterGroup;
        }

        private EncounterGroups.CharacterGroup[] ConfigMonsterGroup(List<string> groupStrings)
        {
            List<EncounterGroups.CharacterGroup> characterGroups = new();
            foreach (var groupString in groupStrings)
            {
                if (!string.IsNullOrEmpty(groupString))
                {
                    var characterGroup = ConfigMonsterDataSOProperties(groupString);
                    characterGroups.Add(characterGroup);
                }
            }

            return characterGroups.ToArray();
        }
    }
}