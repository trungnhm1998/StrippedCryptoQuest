using System.Globalization;
using System.IO;
using System.Linq;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Character.Enemy;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Item;
using CryptoQuest.Item.Consumable;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Gameplay.Monster
{
    public class EnemyImporter : ScriptableObject
    {
        [SerializeField] private TextAsset _enemiesMasterData;
        [SerializeField] private string _exportPath;
        [SerializeField] private CurrencySO _gold;
        [SerializeField] private Elemental[] _elements;
        [SerializeField] private AttributeSets _attributeSets;
        [SerializeField] private ConsumableSO[] _consumables;

        [ContextMenu("Import")]
        public void Import()
        {
            if (string.IsNullOrEmpty(_exportPath))
                SelectExportPath();

            var csvPath = AssetDatabase.GetAssetPath(_enemiesMasterData);
            using var fs = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var stream = new StreamReader(fs);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            using var csv = new CsvReader(stream, config);
            csv.Read();
            csv.ReadHeader();
            csv.Read(); // skip description row
            while (csv.Read())
            {
                var eName = csv.GetField<string>("monster_name").Replace(" ", "");
                var enemy = AssetDatabase.LoadAssetAtPath<EnemyDef>(_exportPath + $"/{eName}.asset");
                if (enemy == null)
                {
                    enemy = CreateInstance<EnemyDef>();
                    AssetDatabase.CreateAsset(enemy, _exportPath + $"/{eName}.asset");
                }

                var enemyDefSO = new SerializedObject(enemy);
                enemyDefSO.Update();
                enemyDefSO.FindProperty("<Id>k__BackingField").intValue = csv.GetField<int>("monster_id");
                var drops = enemyDefSO.FindProperty("_drops");
                drops.ClearArray();
                InsertDrop(ref drops, new ExpLoot { Exp = csv.GetField<uint>("exp") });
                InsertDrop(ref drops, new CurrencyLootInfo(new CurrencyInfo(_gold, csv.GetField<uint>("gold"))));
                InsertConsumableDrop(csv, drops);

                enemyDefSO.ApplyModifiedProperties();
                EditorUtility.SetDirty(enemy);
                AssetDatabase.SaveAssets();
            }
        }

        private SerializedProperty InsertDrop(ref SerializedProperty drops, LootInfo loot, float chance = 1f)
        {
            drops.InsertArrayElementAtIndex(drops.arraySize);
            var element = drops.GetArrayElementAtIndex(drops.arraySize - 1);
            element.FindPropertyRelative("<Loot>k__BackingField").managedReferenceValue = loot;
            element.FindPropertyRelative("<Chance>k__BackingField").floatValue = chance;
            return element;
        }

        private void InsertConsumableDrop(CsvReader csv, SerializedProperty drops)
        {
            if (!csv.TryGetField<string>("drop_item_id", out var dropItemId)) return;
            if (!csv.TryGetField<float>("drop_item_rate", out var dropRate)) return;
            var consumable = _consumables.FirstOrDefault(c => c.ID == dropItemId);
            if (consumable == null) return;
            InsertDrop(ref drops, new ConsumableLootInfo(new ConsumableInfo(consumable)), dropRate);
        }

        [ContextMenu("Select export path")]
        public void SelectExportPath()
        {
            _exportPath = EditorUtility.OpenFolderPanel("Select export path", Application.dataPath, "");
            _exportPath = _exportPath.Replace(Application.dataPath, "Assets");
        }
    }
}