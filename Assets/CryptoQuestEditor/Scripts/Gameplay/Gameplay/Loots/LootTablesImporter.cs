using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Inventory.Currency;
using CryptoQuest.Item.Consumable;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.CryptoQuestEditor.Scripts.Gameplay.Gameplay.Loots
{
    public class LootTablesImporter : ScriptableObject
    {
        [SerializeField] private TextAsset _lootsMasterData;
        [SerializeField] private string _exportPath;
        [SerializeField] private CurrencySO _gold;
        [SerializeField] private ConsumableSO[] _consumables;

        [ContextMenu("Import")]
        public void Import()
        {
            if (string.IsNullOrEmpty(_exportPath))
                SelectExportPath();

            var csvPath = AssetDatabase.GetAssetPath(_lootsMasterData);
            using var fs = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var stream = new StreamReader(fs);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            using var csv = new CsvReader(stream, config);
            csv.Read();
            csv.ReadHeader();
            csv.Read();
            while (csv.Read())
            {
                var lootId = csv.GetField<string>("treasure_id").Replace(" ", "");
                var lootTable = AssetDatabase.LoadAssetAtPath<LootTable>(_exportPath + $"/Loot_{lootId}.asset");
                if (lootTable == null)
                {
                    lootTable = CreateInstance<LootTable>();
                    AssetDatabase.CreateAsset(lootTable, _exportPath + $"/Loot_{lootId}.asset");
                }

                var lootTableSO = new SerializedObject(lootTable);
                lootTableSO.Update();
                lootTableSO.FindProperty("<ID>k__BackingField").intValue = csv.GetField<int>("treasure_id");
                var drops = lootTableSO.FindProperty("LootInfos");
                drops.ClearArray();
                InsertDrop(ref drops, new CurrencyLootInfo(new CurrencyInfo(_gold, csv.GetField<uint>("gold_amount"))));
                InsertConsumableDrop(csv, drops);

                lootTableSO.ApplyModifiedProperties();
                EditorUtility.SetDirty(lootTable);
                AssetDatabase.SaveAssets();
            }
        }

        private void InsertDrop(ref SerializedProperty drops, LootInfo loot, float chance = 1f)
        {
            drops.InsertArrayElementAtIndex(drops.arraySize);
            var element = drops.GetArrayElementAtIndex(drops.arraySize - 1);
            element.boxedValue = loot;
        }

        private void InsertConsumableDrop(CsvReader csv, SerializedProperty drops)
        {
            if (!csv.TryGetField<string>("item_id", out var dropItems)) return;
            var idAmountPairs = ParseItemIdAndAmount(dropItems);
            foreach (var pair in idAmountPairs)
            {
                var consumable = _consumables.FirstOrDefault(c => c.ID.ToString() == pair.Key);
                if (consumable == null) return;
                InsertDrop(ref drops, new ConsumableLootInfo(new ConsumableInfo(consumable, pair.Value)), 1);
            }
        }

        private Dictionary<string, int> ParseItemIdAndAmount(string dropItems)
        {
            Dictionary<string, int> idAmountPairs = new Dictionary<string, int>();
            string[] itemPairs = dropItems.Split(',');
            foreach (string itemPair in itemPairs)
            {
                string[] itemAmountPair = itemPair.Split('x');

                if (itemAmountPair.Length == 2 && !string.IsNullOrEmpty(itemAmountPair[0]) &&
                    int.TryParse(itemAmountPair[1], out int amount))
                {
                    idAmountPairs.Add(itemAmountPair[0], amount);
                }
            }

            return idAmountPairs;
        }

        [ContextMenu("Select export path")]
        public void SelectExportPath()
        {
            _exportPath = EditorUtility.OpenFolderPanel("Select export path", Application.dataPath, "");
            _exportPath = _exportPath.Replace(Application.dataPath, "Assets");
        }
    }
}