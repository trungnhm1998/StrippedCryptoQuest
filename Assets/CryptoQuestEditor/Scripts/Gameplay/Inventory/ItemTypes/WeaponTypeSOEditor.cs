using System;
using System.IO;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Inventory
{
    public class WeaponTypeSOEditor : ScriptableObjectBrowserEditor<WeaponTypeSO>
    {
        private const int ROW_OFFSET = 2;

        public WeaponTypeSOEditor()
        {
            this.createDataFolder = false;

            this.defaultStoragePath = "Assets/ScriptableObjects/Data/Inventory/ItemTypes/Equipments";
        }

        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);

            for (var index = ROW_OFFSET; index < allLines.Length; index++)
            {
                string[] splitedData = allLines[index].Split('\t');
                var id = splitedData[0];
                var weaponType = splitedData[1];
                var name = splitedData[3].Replace(" ", "_");
                var path = this.defaultStoragePath + "/" + name + ".asset";

                if (weaponType != "1")
                    continue;
                WeaponTypeSO instance = null;

                // find instance if null create new
                instance = (WeaponTypeSO)AssetDatabase.LoadAssetAtPath(path, typeof(WeaponTypeSO));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    if (weaponType == "1")
                    {
                        instance = ScriptableObject.CreateInstance<WeaponTypeSO>();
                    }
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