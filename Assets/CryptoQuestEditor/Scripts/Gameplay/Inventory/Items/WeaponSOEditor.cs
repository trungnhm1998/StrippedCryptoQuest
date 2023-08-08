
using System;
using System.IO;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Inventory
{
    public class WeaponSOEditor : ScriptableObjectBrowserEditor<WeaponSO>
    {
        public WeaponSOEditor()
        {
            this.createDataFolder = false;

            this.defaultStoragePath = "Assets/ScriptableObjects/Data/Inventory/Items/Equipments/Weapons";
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
                var name = splitedData[5].Replace(" ", "_");
                var type = splitedData[6];
                var rarity = splitedData[7];
                var path = this.defaultStoragePath + "/" + name + ".asset";

                WeaponSO instance = null;

                // find instance if null create new
                instance = (WeaponSO)AssetDatabase.LoadAssetAtPath(path, typeof(WeaponSO));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<WeaponSO>();
                }

                // import Data
                instance.ID = id;
                instance.name = name;
                instance.WeaponType = (WeaponTypeSO)AssetDatabase.LoadAssetAtPath(
                    "Assets/ScriptableObjects/Data/Inventory/ItemTypes/Equipments/" + type + ".asset",
                    typeof(WeaponTypeSO));


                instance.Rarity = (RaritySO)AssetDatabase.LoadAssetAtPath(
                    "Assets/ScriptableObjects/Data/Inventory/RarityTypes/" + rarity + ".asset", typeof(RaritySO));

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