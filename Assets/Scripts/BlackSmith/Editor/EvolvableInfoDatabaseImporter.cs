using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CryptoQuest.BlackSmith.Evolve;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.BlackSmith
{
    public class EvolvableInfoDatabaseImporter : ScriptableObject
    {
        public class EvolvableInfoImportData
        {
            [Name("eq_evol_id")] public int EvolveId { get; set; }
            [Name("rarity_id")] public int RarityId { get; set; }
            [Name("before_evol_star")] public int BeforeStars { get; set; }
            [Name("after_evol_star")] public int AfterStars { get; set; }
            [Name("preview_minlv")] public int MinLevel { get; set; }
            [Name("preview_maxlv")] public int MaxLevel { get; set; }
            [Name("probability")] public int Rate { get; set; }
            [Name("gold")] public int Gold { get; set; }
            [Name("metaD")] public float Metad { get; set; }
        }

        [SerializeField] private EvolvableInfoDatabaseSO _database;
        [SerializeField] private TextAsset _csvFile;

        [ContextMenu("Import")]
        public void Import()
        {
            var infos = new List<EvolvableInfoImportData>();

            var csvPath = AssetDatabase.GetAssetPath(_csvFile);
            using var fs = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var stream = new StreamReader(fs);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            using var csv = new CsvReader(stream, config);
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var data = csv.GetRecord<EvolvableInfoImportData>();
                infos.Add(new EvolvableInfoImportData()
                {
                    EvolveId = data.EvolveId,
                    RarityId = data.RarityId,
                    BeforeStars = data.BeforeStars,
                    AfterStars = data.AfterStars,
                    MinLevel = data.MinLevel,
                    MaxLevel = data.MaxLevel,
                    Rate = data.Rate,
                    Gold = data.Gold,
                    Metad = data.Metad
                });
            }

            var evolvableInfos = new List<EvolvableInfoData>();
            for (var i = 0; i < infos.Count; i++)
            {
                var info = infos[i];
                var evolvableInfo = new EvolvableInfoData()
                {
                    EvolveId = info.EvolveId,
                    Rarity = info.RarityId,
                    BeforeStars = info.BeforeStars,
                    AfterStars = info.AfterStars,
                    MinLevel = info.MinLevel,
                    MaxLevel = info.MaxLevel,
                    Rate = info.Rate,
                    Gold = info.Gold,
                    Metad = info.Metad
                };
                evolvableInfos.Add(evolvableInfo);
            }

            _database.Editor_SetEvolableInfos(evolvableInfos.ToArray());
            EditorUtility.SetDirty(_database);
            AssetDatabase.SaveAssets();
        }
    }
}