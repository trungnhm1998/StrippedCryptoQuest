using System;
using System.IO;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Inventory
{
    public class EquipmentTypeSOBrowserEditor : ScriptableObjectBrowserEditor<EquipmentTypeSO>
    {
        private const int ROW_OFFSET = 2;
        private const int ROW_NAME = 2;
        private const int ROW_CATEGORY_ID = 3;

        private const string WEAPON_TYPE = "1";

        public EquipmentTypeSOBrowserEditor()
        {
            CreateDataFolder = false;

            DefaultStoragePath = "Assets/ScriptableObjects/Data/Inventory/ItemTypes/Equipments";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] row = File.ReadAllLines(directory);

            for (var index = ROW_OFFSET; index < row.Length; index++)
            {
                // get data form tsv file
                string[] cols = row[index].Split('\t');
                var weaponType = cols[ROW_CATEGORY_ID];
                var name = cols[ROW_NAME].Replace(" ", "_");
                var path = DefaultStoragePath + "/" + name + ".asset";

                if (weaponType == WEAPON_TYPE) continue;

                EquipmentTypeSO instance = null;

                // find instance if null create new
                instance = (EquipmentTypeSO)AssetDatabase.LoadAssetAtPath(path, typeof(EquipmentTypeSO));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<EquipmentTypeSO>();
                }

                // import Data
                instance.name = name;

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
    }
}