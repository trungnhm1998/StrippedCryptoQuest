using System;
using System.Collections.Generic;
using System.IO;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Gameplay.Loot;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Gameplay.Reward
{
    public class LootTableSOEditor : ScriptableObjectBrowserEditor<LootTable>
    {
        private const string DEFAULT_NAME = "LootTable_";
        private const int ROW_OFFSET = 1;

        public LootTableSOEditor()
        {
            CreateDataFolder = false;
            DefaultStoragePath = "Assets/ScriptableObjects/Data/LootTableData";
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

                if (string.IsNullOrEmpty(splitedData[0])) continue;
                bool isRightGoldData = float.TryParse(splitedData[1], out var goldAmount);

                LootTableDataModel dataModel = new()
                {
                    Id = int.Parse(splitedData[0]),
                    GoldAmount = isRightGoldData ? goldAmount : 0,
                };

                LootTable instance = null;
                instance = (LootTable)AssetDatabase.LoadAssetAtPath(path, typeof(LootTable));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<LootTable>();
                }

                instance.Editor_SetUp(dataModel.Id);


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

        #region Set up data

        private List<int> GetRewardItemIds(string stringToSplit)
        {
            string[] itemArr = stringToSplit.Split(',');
            foreach (var itemDef in itemArr)
            {
                
            }
        }

        struct MyStruct
        {
            public int Id;
            public int Amount;
        }

        #endregion
    }
}