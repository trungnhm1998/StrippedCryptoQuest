using System;
using System.Collections.Generic;
using System.IO;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.Encounter;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Gameplay.Monster
{
    public class BattleDataSOEditor : ScriptableObjectBrowserEditor<EncounterData>
    {
        private const string DEFAULT_NAME = "MonsterParty_";
        private const int ROW_OFFSET = 2;
        private const int COLUMN_ENCOUNTER_ID_INDEX = 0;
        private const int COLUMN_BATTLE_BG_ID_INDEX = 2;
        private Dictionary<int, Battlefield> _battlefieldDictionary = new();

        public BattleDataSOEditor()
        {
            CreateDataFolder = false;
            DefaultStoragePath = "Assets/ScriptableObjects/Data/MonsterParty";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);
            LoadAndCacheBattleFields()
            for (int index = ROW_OFFSET; index < allLines.Length; index++)
            {
                // get data form tsv file
                string[] splitedData = allLines[index].Split('\t');
                string name = DEFAULT_NAME + splitedData[0];
                string path = DefaultStoragePath + "/" + name + ".asset";

                EncounterDataModel dataModel = new();
                if (string.IsNullOrEmpty(splitedData[COLUMN_ENCOUNTER_ID_INDEX])) continue;
                dataModel.Id = splitedData[COLUMN_ENCOUNTER_ID_INDEX];

                if (string.IsNullOrEmpty(splitedData[COLUMN_BATTLE_BG_ID_INDEX])) continue;
                dataModel.BackgroundId = splitedData[COLUMN_BATTLE_BG_ID_INDEX];

                List<BattlePartyDataModel> partiesData = GetBattlePartiesDataModel(splitedData);
                if (!IsValidBattlePartiesSetUp(partiesData)) continue;
                dataModel.BattleParties = partiesData;


                EncounterData instance = null;
                instance = (EncounterData)AssetDatabase.LoadAssetAtPath(path, typeof(EncounterData));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<EncounterData>();
                }

                instance.Editor_SetID(dataModel.Id);
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

        private int _startColumnIndex = 7;
        private int _columnOffset = 5;

        private List<BattlePartyDataModel> GetBattlePartiesDataModel(string[] splittedData)
        {
            List<BattlePartyDataModel> battleFieldDataModels = new();
            for (int i = _startColumnIndex; i < _startColumnIndex + _columnOffset; i++)
            {
                BattlePartyDataModel dataModel = new();
                bool isValidId = int.TryParse(splittedData[i], out int partyId);
                bool isValidProb = float.TryParse(splittedData[i + _columnOffset], out float probability);
                if (!isValidId || !isValidProb) continue;
                dataModel.BattleDataId = partyId;
                dataModel.Probability = probability / 100;
                battleFieldDataModels.Add(dataModel);
            }

            return battleFieldDataModels;
        }

        private bool IsValidBattlePartiesSetUp(List<BattlePartyDataModel> datas)
        {
            float totalProbability = 1;
            foreach (var data in datas)
            {
                totalProbability -= data.Probability;
            }

            return totalProbability == 0;
        }

        private void LoadAndCacheBattleFields()
        {
            var guids = AssetDatabase.FindAssets("t:Battlefield");
            foreach (var guid in guids)
            {
                var asset = AssetDatabase.LoadAssetAtPath<Battlefield>(AssetDatabase.GUIDToAssetPath(guid));
                _battlefieldDictionary.TryAdd(asset.Id, asset);
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
        //
        //     return DataValidator.IsValidTotalProbability(monsterGroup);
        // }
        //

        private bool CanSetUpBattleFieldDataModel(BattleFieldDataModel dataModel,
            List<BattleEncounterSetupDataModel> monsterGroup, string[] splitedData)
        {
            int battleFieldId = int.Parse(splitedData[0]);
            string chapterId = splitedData[1];
            bool isBackgroundIdNumeric = int.TryParse(splitedData[5], out int backgroundId);
            if (battleFieldId == null || chapterId == null || !isBackgroundIdNumeric || monsterGroup == null)
            {
                Debug.LogWarning("Invalid data");
                return false;
            }

            dataModel.BattleFieldId = battleFieldId;
            dataModel.BattleEncounterSetups = monsterGroup;
            return true;
        }

        // private AssetReferenceT<Sprite> SetUpBattleBackGround(BattleFieldDataModel dataModel)
        // {
        //     var backgroundAssets = GetAssetsFromType<BattleBackgroundSO>().Where(data
        //         => data.Id == dataModel.BackgroundId);
        //     if (backgroundAssets.Count() == 0) return null;
        //     return backgroundAssets.First().BattleBackground;
        // }


        // private List<int> SetUpInstanceBattleEncounter(BattleFieldDataModel dataModel)
        // {
        //     List<int> battleEncounterSetups = new();
        //     foreach (var battleEncounterSetupDataModel in dataModel.BattleEncounterSetups)
        //     {
        //         var battleDatas = GetAssetsFromType<Battlefield>().Where(data
        //             => data.Id == battleEncounterSetupDataModel.BattleDataId);
        //         if (battleDatas.Count() == 0) continue;
        //         EncounterData.GroupConfig encounterSetup = new();
        //         encounterSetup.Probability = battleEncounterSetupDataModel.Probability /
        //                                      BaseBattleVariable.CORRECTION_PROBABILITY_VALUE;
        //         encounterSetup.Battlefield = battleDatas.First();
        //         battleEncounterSetups.Add(encounterSetup);
        //     }
        //
        //     return battleEncounterSetups;
        // }
    }
}