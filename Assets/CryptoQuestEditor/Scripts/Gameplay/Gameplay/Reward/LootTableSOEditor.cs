using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CryptoQuest.Gameplay.Battle;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Loot;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using IndiGames.Core.Database;

namespace CryptoQuestEditor.Gameplay.Gameplay.Reward
{
    public class LootTableSOEditor : ScriptableObjectBrowserEditor<LootTable>
    {
        private const string DEFAULT_NAME = "LootTable_";
        private const int ROW_OFFSET = 2;
        private const string GOLD_ASSET_PATH = "Assets/ScriptableObjects/Currency/Gold.asset";
        private Dictionary<string, ConsumableSO> _usableItems = new();
        private Dictionary<string, EquipmentSO> _equipmentItems = new();
        private CurrencySO _goldSo;
        private LootDatabase _lootDatabase;
        public LootTableSOEditor()
        {
            CreateDataFolder = false;
            DefaultStoragePath = "Assets/ScriptableObjects/Data/LootTableData";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);
            LoadAndCacheGoldSo();
            LoadAndCacheAllItem();
            LoadAndCacheAllEquipments();
            LoadLootDatabaseSo();
            List<GenericAssetReferenceDatabase<int, LootTable>.Map> maps = new();
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
                LootInfo goldLoot = AddGold(dataModel.GoldAmount);
                if(goldLoot != null)
                    instance.LootInfos.Add(goldLoot);
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
                maps.Add(new()
                {
                    Id = dataModel.Id,
                    Data = new AssetReferenceT<LootTable>(guid),
                });
                instance.SetObjectToAddressableGroup("LootTable");
            }
            _lootDatabase.Editor_SetMaps(maps.ToArray());
            EditorUtility.SetDirty(_lootDatabase); 
        }


        #region Set up data

        private void LoadLootDatabaseSo()
        {
            var guids = AssetDatabase.FindAssets("t:LootDatabase");
            _lootDatabase = AssetDatabase.LoadAssetAtPath<LootDatabase>(AssetDatabase.GUIDToAssetPath(guids[0])); 
        }
        private LootInfo AddGold(float amount)
        {
            if (amount <= 0) return null;
            CurrencyInfo goldInfo = new (_goldSo,amount);
            CurrencyLootInfo goldLootInfo = new CurrencyLootInfo(goldInfo);
            return goldLootInfo;
        }
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

        private void LoadAndCacheGoldSo()
        {
            _goldSo  = AssetDatabase.LoadAssetAtPath(GOLD_ASSET_PATH, typeof(CurrencySO)) as CurrencySO;
        }
            
        private void LoadAndCacheAllItem()
        {
            if (_usableItems.Count == 0)
            {
                var guids = AssetDatabase.FindAssets("t:UsableSO");
                foreach (var guid in guids)
                {
                    var asset = AssetDatabase.LoadAssetAtPath<ConsumableSO>(AssetDatabase.GUIDToAssetPath(guid));
                    if (asset != null && !string.IsNullOrEmpty(asset.ID))
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
                    if (asset != null && !string.IsNullOrEmpty(asset.ID))
                        _equipmentItems.Add(asset.ID, asset);
                }
            }
        }

        private List<LootInfo> SetUpLootInfos(List<RewardDefs> rewardDefs)
        {
            List<LootInfo> lootInfos = new();
            foreach (var rewardDef in rewardDefs)
            {
                Debug.Log(rewardDef.Id + rewardDef.Amount);
                if (_usableItems.TryGetValue(rewardDef.Id, out var item))
                {
                    ConsumableInfo consumableInfo = new ConsumableInfo(item, rewardDef.Amount);
                    UsableLootInfo usableLootInfo = new UsableLootInfo(consumableInfo);
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