using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Encounter;
using ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using EncounterData = CryptoQuest.Gameplay.Battle.EncounterData;

namespace CryptoQuestEditor.Gameplay.Gameplay.Monster
{
    public class BattlefieldDataSOEditor : ScriptableObjectBrowserEditor<EncounterData>
    {
        private const string DEFAULT_NAME = "";
        private const int ROW_OFFSET = 1;

        public BattlefieldDataSOEditor()
        {
            this.createDataFolder = false;
            this.defaultStoragePath = "Assets/ScriptableObjects/Data/BattleFieldData";
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

                List<BattleEncounterSetupDataModel> monsterGroup = new();
                bool isAbleToSetupEncounterData = CanSetUpBattleEncounterModel(monsterGroup, splitedData);
                if (!isAbleToSetupEncounterData) continue;

                BattleFieldDataModel dataModel = new();
                bool isAbleToSetupFieldData = CanSetUpBattleFieldDataModel(dataModel, monsterGroup, splitedData);
                if (!isAbleToSetupFieldData) continue;

                EncounterData instance = null;
                instance = (EncounterData)AssetDatabase.LoadAssetAtPath(path, typeof(EncounterData));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<EncounterData>();
                }

                instance.Editor_SetBattlefieldID(dataModel.BattleFieldId);

                var battleEncounterSetups = SetUpInstanceBattleEncounter(dataModel);
                instance.Editor_SetConfig(battleEncounterSetups);

                var background = SetUpBattleBackGround(dataModel);
                instance.Background = background;
                instance.name = name;
                if (!DataValidator.IsCorrectBattleFieldSetup(instance)) continue;
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

        private List<EncounterData.GroupConfig> SetUpInstanceBattleEncounter(BattleFieldDataModel dataModel)
        {
            List<EncounterData.GroupConfig> battleEncounterSetups = new();
            foreach (var battleEncounterSetupDataModel in dataModel.BattleEncounterSetups)
            {
                var battleDatas = GetAssetsFromType<EnemyParty>().Where(data
                    => data.Id == battleEncounterSetupDataModel.BattleDataId);
                if (battleDatas.Count() == 0) continue;
                EncounterData.GroupConfig encounterSetup = new();
                encounterSetup.Probability = battleEncounterSetupDataModel.Probability /
                                             BaseBattleVariable.CORRECTION_PROBABILITY_VALUE;
                encounterSetup.Party = battleDatas.First();
                battleEncounterSetups.Add(encounterSetup);
            }

            return battleEncounterSetups;
        }

        private AssetReferenceT<Sprite> SetUpBattleBackGround(BattleFieldDataModel dataModel)
        {
            var backgroundAssets = GetAssetsFromType<BattleBackgroundSO>().Where(data
                => data.Id == dataModel.BackgroundId);
            if (backgroundAssets.Count() == 0) return null;
            return backgroundAssets.First().BattleBackground;
        }

        #region Set up data

        private const int PARTY_ID_FIRST_INDEX = 7;
        private const int ID_PROBABILITY_COLUMN_JUMP = 5;

        private bool CanSetUpBattleEncounterModel(List<BattleEncounterSetupDataModel> monsterGroup,
            string[] splitedData)
        {
            for (int i = PARTY_ID_FIRST_INDEX; i < (PARTY_ID_FIRST_INDEX + ID_PROBABILITY_COLUMN_JUMP); i++)
            {
                int probabilityIndex = i + ID_PROBABILITY_COLUMN_JUMP;
                bool isValidPair = DataValidator.IsValidPair(splitedData[i], splitedData[probabilityIndex]);
                if (!isValidPair)
                {
                    Debug.LogWarning("Invalid pair of monster id and probability");
                    return false;
                }

                monsterGroup.Add(
                    new BattleEncounterSetupDataModel(int.Parse(splitedData[i]),
                        float.Parse(splitedData[probabilityIndex])));
            }

            return DataValidator.IsValidTotalProbability(monsterGroup);
        }

        private bool CanSetUpBattleFieldDataModel(BattleFieldDataModel dataModel,
            List<BattleEncounterSetupDataModel> monsterGroup, string[] splitedData)
        {
            string battleFieldId = splitedData[0];
            string chapterId = splitedData[1];
            bool isBackgroundIdNumeric = int.TryParse(splitedData[5], out int backgroundId);
            if (battleFieldId == null || chapterId == null || !isBackgroundIdNumeric || monsterGroup == null)
            {
                Debug.LogWarning("Invalid data");
                return false;
            }

            dataModel.BattleFieldId = battleFieldId;
            dataModel.ChapterId = chapterId;
            dataModel.BackgroundId = backgroundId;
            dataModel.BattleEncounterSetups = monsterGroup;
            return true;
        }

        #endregion
    }
}