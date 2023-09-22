using System;
using System.IO;
using System.Linq;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Item.Equipment;
using CryptoQuestEditor.Helper;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace CryptoQuestEditor
{
    public class EquipmentSOBrowserEditor : ScriptableObjectBrowserEditor<EquipmentPrefab>
    {
        private const int ROW_OFFSET = 2;

        private const string DEFAULT_STORAGE_PATH = "Assets/ScriptableObjects/Data/Inventory/Items/Equipments";

        private const int ROW_ID = 1;
        private const int ROW_LOCALIZED_NAME = 2;
        private const int ROW_EQUIPMENT_TYPE_ID = 15;
        private const int ROW_CHARACTER_LEVEL_REQUIREMENT = 20;
        private const int ROW_EQUIPMENT_SLOT_ID = 14;

        private readonly string[] _tableNames = { "Equipment", "Weapon", "Armor", "Accessory" };

        public EquipmentSOBrowserEditor()
        {
            CreateDataFolder = false;

            DefaultStoragePath = "Assets/ScriptableObjects/Data/Inventory/Items/Equipments";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] rows = File.ReadAllLines(directory);

            for (int index = ROW_OFFSET; index < rows.Length; index++)
            {
                // get data form tsv file
                string[] cols = rows[index].Split('\t');

                Debug.Log($"Importing {cols}");
                
                string id = cols[ROW_ID];

                string currentID = String.Empty;
                if (currentID == id) continue;
                currentID = id;

                var slot = GetEquipmentSlot(ParseData(cols[ROW_EQUIPMENT_SLOT_ID]));

                string name = $"{GetNameBySlot(slot)}_{id}";
                string path = $"{GetPathBySlot(slot)}/{name}.asset";

                EquipmentPrefab instance = null;

                // find instance if null create new
                instance = (EquipmentPrefab)AssetDatabase.LoadAssetAtPath(path, typeof(EquipmentPrefab));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<EquipmentPrefab>();
                }

                // import Data
                instance.Editor_SetID(id);
                instance.name = name;

                instance.Editor_SetEquipmentType(GetEquipmentType(ParseData(cols[ROW_EQUIPMENT_TYPE_ID])));
                instance.Editor_SetRequiredCharacterLevel(ParseData(cols[ROW_CHARACTER_LEVEL_REQUIREMENT]));
                instance.Editor_SetRequiredSlots(GetRequiredSlots(cols[ROW_EQUIPMENT_SLOT_ID]));

                var displayName = LocalizedString(cols, out var description);

                instance.Editor_SetDisplayName(displayName);
                instance.Editor_SetDescription(description);

                // Save data
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

        private LocalizedString LocalizedString(string[] rows, out LocalizedString description)
        {
            LocalizedString displayName = new LocalizedString();
            description = new LocalizedString();

            StringTableCollection table = null;
            SharedTableData.SharedTableEntry tableEntry = null;

            foreach (var tableName in _tableNames)
            {
                table = LocalizationEditorSettings.GetStringTableCollection(tableName);
                if (table == null) continue;

                tableEntry = table.SharedData.GetEntryFromReference(rows[ROW_LOCALIZED_NAME]);

                if (tableEntry != null) break;
            }

            if (tableEntry == null) return displayName;

            displayName.TableReference = table.TableCollectionNameReference;
            description.TableReference = table.TableCollectionNameReference;
            displayName.TableEntryReference = tableEntry.Id;
            description.TableEntryReference = tableEntry.Id;

            return displayName;
        }


        private string GetNameBySlot(EquipmentSlot.EType slot)
        {
            switch (slot)
            {
                default:
                case EquipmentSlot.EType.LeftHand:
                    return "Weapon";
                case EquipmentSlot.EType.RightHand:
                    return "Shield";
                case EquipmentSlot.EType.Body:
                    return "Armor";
                case EquipmentSlot.EType.Foot:
                    return "Shoe";
                case EquipmentSlot.EType.Head:
                    return "Helmet";
                case EquipmentSlot.EType.Leg:
                    return "Trouser";
                case EquipmentSlot.EType.Accessory1:
                case EquipmentSlot.EType.Accessory2:
                    return "Accessory";
            }
        }

        private string GetPathBySlot(EquipmentSlot.EType slot)
        {
            switch (slot)
            {
                default:
                case EquipmentSlot.EType.LeftHand:
                case EquipmentSlot.EType.RightHand:
                    return DEFAULT_STORAGE_PATH + "/Weapons";

                case EquipmentSlot.EType.Head:
                    return DEFAULT_STORAGE_PATH + "/Helmet";

                case EquipmentSlot.EType.Body:
                    return DEFAULT_STORAGE_PATH + "/Armor";

                case EquipmentSlot.EType.Leg:
                    return DEFAULT_STORAGE_PATH + "/Shoe";

                case EquipmentSlot.EType.Foot:
                    return DEFAULT_STORAGE_PATH + "/Trousers";

                case EquipmentSlot.EType.Accessory1:
                case EquipmentSlot.EType.Accessory2:
                    return DEFAULT_STORAGE_PATH + "/Accessories";
            }
        }

        private EquipmentSlot.EType[] GetRequiredSlots(string data)
        {
            string[] rows = data.Split(',');
            EquipmentSlot.EType[] slots = new EquipmentSlot.EType[rows.Length];

            for (int i = 0; i < rows.Length; i++)
            {
                if (i == rows.Length - 1 || i == rows.Length - 2)
                {
                    int newSize = 2;
                    Array.Resize(ref slots, newSize);
                    slots[^2] = EquipmentSlot.EType.Accessory1;
                    slots[^1] = EquipmentSlot.EType.Accessory2;
                }
                else
                {
                    slots[i] = GetEquipmentSlot(ParseData(rows[i]));
                }
            }

            return slots;
        }

        private EquipmentTypeSO GetEquipmentType(int id)
        {
            var allTypes = ToolsHelper.GetAssets<EquipmentTypeSO>();

            return allTypes.FirstOrDefault(type => type.Id == id);
        }

        private int ParseData(string data) => string.IsNullOrEmpty(data) ? 0 : int.Parse(data);
        private EquipmentSlot.EType GetEquipmentSlot(int value) => (EquipmentSlot.EType)value;
    }
}