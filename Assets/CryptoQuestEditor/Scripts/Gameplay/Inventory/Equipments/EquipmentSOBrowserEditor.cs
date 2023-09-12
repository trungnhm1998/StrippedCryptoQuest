using System;
using System.Collections.Generic;
using System.IO;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuestEditor.Helper;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

namespace CryptoQuestEditor
{
    public class EquipmentSOBrowserEditor : ScriptableObjectBrowserEditor<EquipmentSO>
    {
        private const string DEFAULT_NAME = "Equipment";
        private const int ROW_OFFSET = 2;

        private const string DEFAULT_STORAGE_PATH = "Assets/ScriptableObjects/Data/Inventory/Items/Equipments";

        private const int ROW_ID = 0;
        private const int ROW_EQUIPMENT_TYPE_ID = 14;
        private const int ROW_CATEGORY_ID = 15;
        private const int ROW_CHARACTER_LEVEL_REQUIREMENT = 19;
        private const int ROW_EQUIPMENT_SLOT_ID = 54;
        
        private Dictionary<string, string> map = new Dictionary<string, string>()
        {
            {"attack", "Assets/ScriptableObjects/Battle/Characters/Attributes/Default.CriticalRate.asset"}
        };

        public EquipmentSOBrowserEditor()
        {
            CreateDataFolder = false;

            DefaultStoragePath = "Assets/ScriptableObjects/Data/Inventory/Items/Equipments";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);

            for (int index = ROW_OFFSET; index < allLines.Length; index++)
            {
                // get data form tsv file
                string[] splitedData = allLines[index].Split('\t');
                string id = splitedData[ROW_ID];
                string name = $"{DEFAULT_NAME}_{id}";
                var slot = GetEquipmentSlot(ParseData(splitedData[ROW_EQUIPMENT_SLOT_ID]));

                string path = $"{GetPathBySlot(slot)}/{name}.asset";

                EquipmentSO instance = null;

                // find instance if null create new
                instance = (EquipmentSO)AssetDatabase.LoadAssetAtPath(path, typeof(EquipmentSO));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<EquipmentSO>();
                }

                var col_name = "min_luck";
                var attributeName = col_name.Split("_")[1];
                var attributePath = map[attributeName];
                // load using asset database
                // var attribute = (AttributeSO)AssetDatabase.LoadAssetAtPath(attributePath, typeof(AttributeSO));

                // import Data
                instance.Editor_SetID(id);
                instance.name = name;
                instance.Editor_SetEquipmentType(GetEquipmentType(ParseData(splitedData[ROW_EQUIPMENT_TYPE_ID])));
                // instance.Editor_SetRarity(GetRarity(ParseData(splitedData[ROW_CATEGORY_ID])));
                instance.Editor_SetRequiredCharacterLevel(ParseData(splitedData[ROW_CHARACTER_LEVEL_REQUIREMENT]));
                instance.Editor_SetRequiredSlots(GetRequiredSlots(splitedData[ROW_EQUIPMENT_SLOT_ID]));

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
            string[] splitedData = data.Split(',');
            EquipmentSlot.EType[] slots = new EquipmentSlot.EType[splitedData.Length];

            for (int i = 0; i < splitedData.Length; i++)
            {
                slots[i] = GetEquipmentSlot(ParseData(splitedData[i]));
            }

            return slots;
        }

        private EquipmentTypeSO GetEquipmentType(int id)
        {
            var allTypes = ToolsHelper.GetAssets<EquipmentTypeSO>();

            foreach (var type in allTypes)
            {
                if (type.Id != id) continue;
                return type;
            }

            return null;
        }

        private RaritySO GetRarity(int id)
        {
            var allRarities = ToolsHelper.GetAssets<RaritySO>();

            foreach (var rarity in allRarities)
            {
                if (rarity.ID != id) continue;
                return rarity;
            }

            return null;
        }

        private int ParseData(string data) => string.IsNullOrEmpty(data) ? 0 : int.Parse(data);
        private EquipmentSlot.EType GetEquipmentSlot(int value) => (EquipmentSlot.EType)value;
    }
}