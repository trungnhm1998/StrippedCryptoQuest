using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using CryptoQuest.BlackSmith.ScriptableObjects;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.BlackSmith
{
    public class UpgradeCostDatabaseImporter : ScriptableObject
    {
        public class UpgradeCostData
        {
            [Name("rarity_id")] public int RarityId { get; set; }
            [Name("gold")] public int Gold { get; set; }
        }

        [SerializeField] private UpgradeCostDatabase _database;
        [SerializeField] private TextAsset _csvFile;

        [ContextMenu("Import")]
        public void Import()
        {
            var csvPath = AssetDatabase.GetAssetPath(_csvFile);
            using var fs = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var stream = new StreamReader(fs);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            using var csv = new CsvReader(stream, config);
            csv.Read();
            csv.ReadHeader();

            var rarityIds = new List<int>();
            var costDatas = new List<CostByRarity>();
            var costs = new List<int>();

            while (csv.Read())
            {
                var data = csv.GetRecord<UpgradeCostData>();
                if (!rarityIds.Contains(data.RarityId))
                {
                    if (costs.Count > 0)
                        costDatas.Add(new CostByRarity()
                        {
                            RarityID = rarityIds[^1],
                            Costs = costs.ToArray()
                        });

                    rarityIds.Add(data.RarityId);
                    costs = new List<int>();
                }

                costs.Add(data.Gold);
            }

            costDatas.Add(new CostByRarity()
            {
                RarityID = rarityIds[^1],
                Costs = costs.ToArray()
            });

            _database.Editor_SetCostData(costDatas.ToArray());

            EditorUtility.SetDirty(_database);
            AssetDatabase.SaveAssets();
        }
    }
}