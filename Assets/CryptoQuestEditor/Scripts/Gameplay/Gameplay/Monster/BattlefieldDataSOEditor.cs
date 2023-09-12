using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation;
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

        public BattlefieldDataSOEditor()
        {
            CreateDataFolder = false;
            DefaultStoragePath = "Assets/ScriptableObjects/Data/Battlefields";
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
                var enemyGroup = dataModel.BattleEncounterSetups;
                instance.Editor_SetEnemyGroups(enemyGroup.ToArray());
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
            return dataModel.BattleEncounterSetups.Count > 0;
        }
      
      

        #endregion
    }
}