using System;
using System.IO;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using ScriptableObjectBrowser;
using UnityEditor;

using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Inventory
{
    public class EquipmentTypeSOEditor : ScriptableObjectBrowserEditor<EquipmentTypeSO>
    {
        public EquipmentTypeSOEditor()
        {
            this.createDataFolder = false;

            this.defaultStoragePath = "Assets/ScriptableObjects/Data/Inventory/ItemTypes/Equipments";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);
            bool isSkippedFirstLine = false;

            foreach (var line in allLines)
            {
                if (!isSkippedFirstLine)
                {
                    isSkippedFirstLine = true;
                    continue;
                }

                // get data form tsv file
                string[] splitedData = line.Split('\t');
                var id = splitedData[0];
                var weaponType = splitedData[1];
                var name = splitedData[3].Replace(" ", "_");
                var path = this.defaultStoragePath + "/" + name + ".asset";

                if (weaponType == "1") continue;

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
                if (instance == null || !AssetDatabase.Contains(instance))
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