using System.Globalization;
using System.IO;
using CryptoQuest.Shop;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Shop.Editor
{
    public class ShopDatabaseImporter : ScriptableObject
    {
        [SerializeField] private ItemPriceMappingDatabase _database;
        [SerializeField] private TextAsset[] _itemCsvFiles;

        [ContextMenu("Import")]
        public void Import()
        {
            var databaseSO = new SerializedObject(_database);
            databaseSO.Update();
            var priceMappings = databaseSO.FindProperty("_priceMappings");
            priceMappings.ClearArray();
            databaseSO.ApplyModifiedProperties();

            foreach (var csvFile in _itemCsvFiles)
            {
                var csvPath = AssetDatabase.GetAssetPath(csvFile);
                using var fs = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var stream = new StreamReader(fs);

                var config = new CsvConfiguration(CultureInfo.InvariantCulture);
                using var csv = new CsvReader(stream, config);
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    if (!csv.TryGetField<string>("equipment_id", out var itemId))
                        itemId = csv.GetField<string>("item_id");
                    if (csv.TryGetField<float>("price", out var buyingPrice) == false) continue;
                    if (csv.TryGetField<float>("selling_price", out var sellingPrice) == false) continue;

                    if (buyingPrice == 0 || sellingPrice == 0) continue;
                    priceMappings.InsertArrayElementAtIndex(priceMappings.arraySize);
                    var itemPriceMapping = priceMappings.GetArrayElementAtIndex(priceMappings.arraySize - 1);
                    itemPriceMapping.FindPropertyRelative("ItemId").stringValue = itemId;
                    itemPriceMapping.FindPropertyRelative("BuyingPrice").floatValue = buyingPrice;
                    itemPriceMapping.FindPropertyRelative("SellingPrice").floatValue = sellingPrice;
                    databaseSO.ApplyModifiedProperties();
                }
            }

            AssetDatabase.SaveAssets();
        }
    }
}