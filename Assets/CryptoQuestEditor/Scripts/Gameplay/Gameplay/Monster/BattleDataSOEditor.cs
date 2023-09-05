using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CryptoQuest.Gameplay.BaseGameplayData;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Encounter;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Gameplay.Monster
{
    public class BattleDataSOEditor : ScriptableObjectBrowserEditor<EnemyParty>
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

                EnemyParty instance = null;
                instance = (EnemyParty)AssetDatabase.LoadAssetAtPath(path, typeof(EnemyParty));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<EnemyParty>();
                }

                List<string> groups = new()
                {
                    splitedData[2], splitedData[3], splitedData[4], splitedData[5]
                };
                if (!DataValidator.IsValidNumberOfMonsterSetup(groups)) continue;
                // instance.Editor_SetEnemyGroups(ConfigMonsterGroup(groups)); // TODO: REFACTOR ENCOUNTER
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

        /*
        private EncounterGroup.CharacterGroup ConfigMonsterDataSOProperties(string groupProperties)
        {
            string[] splitArray = groupProperties.Split(",", StringSplitOptions.None);
            List<CharacterData> monsterDataGroup = new();
            foreach (var id in splitArray)
            {
                if (string.IsNullOrEmpty(id)) continue;
                var assets = GetAssetsFromType<MonsterData>().Where(monster
                    => monster.MonsterId == int.Parse(id));
                MonsterData monsterData = assets.Count() > 0 ? assets.First() : null;
                if (monsterData != null)
                {
                    monsterDataGroup.Add(monsterData);
                }
            }

            EncounterGroup.CharacterGroup characterGroup = new();
            // TODO: REFACTOR ENCOUNTER
            // characterGroup.Editor_SetCharacters(monsterDataGroup.ToArray());
            return characterGroup;
        }

        private EncounterGroup.CharacterGroup[] ConfigMonsterGroup(List<string> groupStrings)
        {
            List<EncounterGroup.CharacterGroup> characterGroups = new();
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
        */
    }
}