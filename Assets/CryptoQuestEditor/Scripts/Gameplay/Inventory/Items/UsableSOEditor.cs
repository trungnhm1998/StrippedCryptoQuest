using System;
using System.IO;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Inventory
{
    public class UsableSOEditor : ScriptableObjectBrowserEditor<UsableSO>
    {
        public UsableSOEditor()
        {
            this.createDataFolder = false;

            this.defaultStoragePath = "Assets/ScriptableObjects/Data/Inventory/Items/Usables";
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
                var name = splitedData[2].Replace(" ", "_");
                var type = splitedData[5];
                var path = this.defaultStoragePath + "/" + name + ".asset";

                UsableSO instance = null;

                // find instance if null create new
                instance = (UsableSO)AssetDatabase.LoadAssetAtPath(path, typeof(UsableSO));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<UsableSO>();
                }

                var keyAsset =
                    (UsableTypeSO)AssetDatabase.LoadAssetAtPath(
                        "Assets/ScriptableObjects/Data/Inventory/UsableTypes/KeyItem.asset", typeof(UsableTypeSO));
                var consumableAsset =
                    (UsableTypeSO)AssetDatabase.LoadAssetAtPath(
                        "Assets/ScriptableObjects/Data/Inventory/UsableTypes/Consumable.asset", typeof(UsableTypeSO));

                // import Data
                instance.ID = id;
                instance.name = name;

                instance.UsableTypeSO = consumableAsset;
                if (type == "1")
                {
                    instance.UsableTypeSO = keyAsset;
                }

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