using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CryptoQuest.Ranch.ScriptableObject;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Ranch.Editor
{
    public class BeastUpgradeCostDatabaseImporter : ScriptableObject
    {
        public class BeastUpgradeCostData
        {
            [Name("gold")] public int Gold { get; set; }
        }

        [SerializeField] private BeastUpgradeCostDatabase _database;
        [SerializeField] private TextAsset _csvFile;


        [ContextMenu("Import")]
        public void Import()
        {
            var csvPath = AssetDatabase.GetAssetPath(_csvFile);
            using var fs = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var stream = new StreamReader(fs);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            using var csv = new CsvReader(stream, config);

            csv.Read();
            csv.ReadHeader();

            var costDatas = new BeastCost();
            var costs = new List<int>();

            while (csv.Read())
            {
                var data = csv.GetRecord<BeastUpgradeCostData>();
                costs.Add(data.Gold);
            }

            costDatas.Costs = costs.ToArray();
            _database.Editor_SetCostData(costDatas);

            EditorUtility.SetDirty(_database);
            AssetDatabase.SaveAssets();
        }
    }
}