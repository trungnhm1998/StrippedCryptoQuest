using System;
using System.Collections.Generic;
using System.IO;
using CryptoQuest.Core;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.Encounter;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuestEditor.Gameplay.Gameplay.Monster
{
    public class BattleDataSOEditor : ScriptableObjectBrowserEditor<EncounterData>
    {
        private const string DEFAULT_NAME = "";
        private const int ROW_OFFSET = 2;
        private const int COLUMN_ENCOUNTER_ID_INDEX = 0;
        private const int COLUMN_BATTLE_BG_NAME_INDEX = 6;
        private Dictionary<int, Battlefield> _battlefieldDictionary = new();
        private EncounterDatabase _encounterDatabase;
        private Dictionary<string, Sprite> _backgrounds = new();
        private const string BATTLEFIELD_BACKGROUND_DATA_PATH = "Assets/Arts/UI/Battle/Backgrounds";

        public BattleDataSOEditor()
        {
            CreateDataFolder = false;
            DefaultStoragePath = "Assets/ScriptableObjects/Data/EncounterDatabase";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);
            LoadAndCacheBattleFields();
            LoadAndCacheEncounterDatabase();
            LoadAndCacheBackgrounds();
            List<GenericAssetReferenceDatabase<string, EncounterData>.Map> maps = new();
            for (int index = ROW_OFFSET; index < allLines.Length; index++)
            {
                // get data form tsv file
                string[] splitedData = allLines[index].Split('\t');
                string name = DEFAULT_NAME + splitedData[0];
                string path = DefaultStoragePath + "/" + name + ".asset";

                EncounterDataModel dataModel = new();
                if (string.IsNullOrEmpty(splitedData[COLUMN_ENCOUNTER_ID_INDEX])) continue;
                dataModel.Id = splitedData[COLUMN_ENCOUNTER_ID_INDEX];


                if (string.IsNullOrEmpty(splitedData[COLUMN_BATTLE_BG_NAME_INDEX])) continue;
                dataModel.BackgroundName = splitedData[COLUMN_BATTLE_BG_NAME_INDEX];


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
                instance.Editor_SetGroup(GetGroupConfigs(dataModel));
                instance.Editor_SetBackground(GetBackgroundSprite(dataModel.BackgroundName));
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


                var guid = AssetDatabase.AssetPathToGUID(path);
                maps.Add(new GenericAssetReferenceDatabase<string, EncounterData>.Map()
                {
                    Id = dataModel.Id,
                    Data = new AssetReferenceT<EncounterData>(guid)
                });
                instance.SetObjectToAddressableGroup("EncounterData");
            }

            _encounterDatabase.Editor_SetMaps(maps.ToArray());
            EditorUtility.SetDirty(_encounterDatabase);
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

        private void LoadAndCacheEncounterDatabase()
        {
            var guid = AssetDatabase.FindAssets("t:EncounterDatabase")[0];
            _encounterDatabase = AssetDatabase.LoadAssetAtPath<EncounterDatabase>(AssetDatabase.GUIDToAssetPath(guid));
        }

        private Sprite GetBackgroundSprite(string name)
        {
            bool isFound = _backgrounds.TryGetValue(name.ToLower(), out var result);
            return isFound ? result : null;
        }

        private List<EncounterData.GroupConfig> GetGroupConfigs(EncounterDataModel data)
        {
            List<EncounterData.GroupConfig> groupConfigs = new();
            foreach (var party in data.BattleParties)
            {
                if (_battlefieldDictionary.TryGetValue(party.BattleDataId, out var battleField))
                    groupConfigs.Add(new EncounterData.GroupConfig()
                    {
                        Probability = party.Probability,
                        Battlefield = battleField
                    });
            }

            return groupConfigs;
        }

        private bool IsValidBattlePartiesSetUp(List<BattlePartyDataModel> datas)
        {
            float totalProbability = 1;
            foreach (var data in datas)
            {
                totalProbability -= data.Probability;
                totalProbability = (float)Math.Round(totalProbability, 2);
            }

            return totalProbability == 0;
        }

        private void LoadAndCacheBackgrounds()
        {
            string[] fileEntries = Directory.GetFiles("Assets/Arts/UI/Battle/Backgrounds");
            foreach (string fileName in fileEntries)
            {
                string path = fileName.Replace(@"\", "/");
                if (path.Contains(".meta")) continue;
                var asset = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                _backgrounds.TryAdd(asset.name.ToLower(), asset);
            }
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
    }
}