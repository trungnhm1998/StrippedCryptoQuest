using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Encounter;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using EncounterData = CryptoQuest.Gameplay.Battle.EncounterData;

namespace CryptoQuestEditor.Gameplay.Gameplay.Monster
{
    public class BattlefieldDataSOEditor : ScriptableObjectBrowserEditor<Battlefield>
    {
        private const string DEFAULT_NAME = "";
        private const int ROW_OFFSET = 1;
        private const string BATTLE_AREA_TYPE = "Battle Area";
        private const string EVENT_BATTLE_TYPE = "Event Battle";

        public BattlefieldDataSOEditor()
        {
            CreateDataFolder = false;
            DefaultStoragePath = "Assets/ScriptableObjects/Battlefields";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);

            for (int index = ROW_OFFSET; index < allLines.Length; index++)
            {
                // get data form tsv file
                string[] splitedData = allLines[index].Split('\t');
                string name = DEFAULT_NAME + splitedData[0];
                string path = DefaultStoragePath + "/" + name + ".asset";

                BattleFieldDataModel dataModel = new();
                bool isDataModelSetupSuccess = CanSetUpBattleFieldModel(dataModel, splitedData);
                if (!isDataModelSetupSuccess) continue;

                Battlefield instance = null;
                instance = (Battlefield)AssetDatabase.LoadAssetAtPath(path, typeof(Battlefield));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<Battlefield>();
                }

                instance.Editor_SetId(dataModel.BattleFieldId);
                var enemyGroup = dataModel.BattleEnemyGroups;
                instance.Editor_SetEnemyGroups(enemyGroup);

                var canRetreat = dataModel.BattleType == BattleFieldDataModel.EBattleType.BattleArea;
                instance.Editor_SetRetreat(canRetreat);

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


        private List<int> GetPartyMonsterIds(string groupProperties)
        {
            List<int> partyIds = new();
            string[] splitArray = groupProperties.Split(",", StringSplitOptions.None);

            foreach (var idString in splitArray)
            {
                bool isValid = int.TryParse(idString, out int id);
                if (!isValid) continue;
                partyIds.Add(id);
            }

            return partyIds;
        }
       


        #region Set up data

        private bool CanSetUpBattleFieldModel(BattleFieldDataModel dataModel, string[] splitedData)
        {
            bool isIdValid = int.TryParse(splitedData[0], out int fieldId);
            if (!isIdValid) return false;
            dataModel.BattleFieldId = fieldId;
            dataModel.BattleEncounterSetups = GetPartyMonsterIds(splitedData[1]);
            dataModel.BattleEnemyGroups.Clear();
            GetMonsterGroupData(dataModel, splitedData[2]);
            GetMonsterGroupData(dataModel, splitedData[3]);
            GetMonsterGroupData(dataModel, splitedData[4]);
            GetMonsterGroupData(dataModel, splitedData[5]);

            dataModel.BattleType = BattleFieldDataModel.EBattleType.BattleArea;
            if (splitedData[6] == EVENT_BATTLE_TYPE)
            {
                dataModel.BattleType = BattleFieldDataModel.EBattleType.EventBattle;
            }
            return dataModel.BattleEncounterSetups.Count > 0;
        }

        private void GetMonsterGroupData(BattleFieldDataModel dataModel, string group)
        {
            var groups = GetPartyMonsterIds(group);
            if (groups.Count <= 0) return;
            dataModel.BattleEnemyGroups.Add(groups.ToArray());
        }

        #endregion
    }
}