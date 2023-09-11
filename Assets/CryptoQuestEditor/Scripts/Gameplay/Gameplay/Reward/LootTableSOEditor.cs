using System;
using System.Collections.Generic;
using System.IO;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Loot;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Gameplay.Reward
{
    public class LootTableSOEditor : ScriptableObjectBrowserEditor<LootTable>
    {
        private const string DEFAULT_NAME = "LootTable_";
        private const int ROW_OFFSET = 2;
        private Dictionary<string, UsableSO> _usableItems = new();
        private Dictionary<string, EquipmentSO> _equipmentItems = new();

        public LootTableSOEditor()
        {
            CreateDataFolder = false;
            DefaultStoragePath = "Assets/ScriptableObjects/Data/LootTableData";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);
            LoadAndCacheAllItem();
            LoadAndCacheAllEquipments();
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
                    RewardDefs = GetRewardItemIds(splitedData[2]),
                };

                LootTable instance = null;
                instance = (LootTable)AssetDatabase.LoadAssetAtPath(path, typeof(LootTable));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<LootTable>();
                }

                instance.Editor_SetUp(dataModel.Id);
                instance.LootInfos = SetUpLootInfos(dataModel.RewardDefs);

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


        #region Set up data

        private List<RewardDefs> GetRewardItemIds(string stringToSplit)
        {
            List<RewardDefs> itemMaps = new();
            string[] itemArr = stringToSplit.Split(',');
            foreach (var itemDef in itemArr)
            {
                string[] itemDefArr = itemDef.Split('x');
                itemMaps.Add(new RewardDefs()
                {
                    Id = itemDefArr[0],
                    Amount = int.Parse(itemDefArr[1]),
                });
            }

            return itemMaps;
        }

        private void LoadAndCacheAllItem()
        {
            if (_usableItems.Count == 0)
            {
                var guids = AssetDatabase.FindAssets("t:UsableSO");
                foreach (var guid in guids)
                {
                    var asset = AssetDatabase.LoadAssetAtPath<UsableSO>(AssetDatabase.GUIDToAssetPath(guid));
                    if (asset != null && string.IsNullOrEmpty(asset.ID))
                        _usableItems.Add(asset.ID, asset);
                }
            }
        }

        private void LoadAndCacheAllEquipments()
        {
            if (_equipmentItems.Count == 0)
            {
                var guids = AssetDatabase.FindAssets("t:EquipmentSO");
                foreach (var guid in guids)
                {
                    var asset = AssetDatabase.LoadAssetAtPath<EquipmentSO>(AssetDatabase.GUIDToAssetPath(guid));
                    if (asset != null && string.IsNullOrEmpty(asset.ID))
                        _equipmentItems.Add(asset.ID, asset);
                }
            }
        }

        private List<LootInfo> SetUpLootInfos(List<RewardDefs> rewardDefs)
        {
            List<LootInfo> lootInfos = new();
            foreach (var rewardDef in rewardDefs)
            {
                if (_usableItems.TryGetValue(rewardDef.Id, out var item))
                {
                    UsableInfo usableInfo = new UsableInfo(item, rewardDef.Amount);
                    UsableLootInfo usableLootInfo = new UsableLootInfo(usableInfo);
                    lootInfos.Add(usableLootInfo);
                }

                if (_equipmentItems.TryGetValue(rewardDef.Id, out var equipment))
                {
                    for (int i = 0; i < rewardDef.Amount; i++)
                    {
                        EquipmentInfo equipmentInfo = new EquipmentInfo(equipment);
                        EquipmentLootInfo equipmentLootInfo = new EquipmentLootInfo(equipmentInfo);
                        lootInfos.Add(equipmentLootInfo);
                    }
                }
            }

            return lootInfos;
        }

        #endregion
    }
}