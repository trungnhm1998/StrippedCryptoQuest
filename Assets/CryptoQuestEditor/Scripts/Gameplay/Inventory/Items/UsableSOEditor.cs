using System;
using System.IO;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Inventory
{
    public class UsableSOEditor : ScriptableObjectBrowserEditor<UsableSO>
    {
        private const string DEFAULT_NAME = "Usable";
        private const int ROW_OFFSET = 2;

        public UsableSOEditor()
        {
            CreateDataFolder = false;

            DefaultStoragePath = "Assets/ScriptableObjects/Data/Inventory/Items/Usables";
        }

        /// <summary>
        /// <see cref="UsableSO.Editor_SetUsableType"/>
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="callback"></param>
        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);

            for (int index = ROW_OFFSET; index < allLines.Length; index++)
            {
                // get data form tsv file
                string[] splitedData = allLines[index].Split('\t');
                string id = splitedData[0];
                string name = DEFAULT_NAME + id;
                string type = splitedData[5];
                string path = DefaultStoragePath + "/" + name + ".asset";

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
                instance.Editor_SetID(id);
                instance.name = name;

                instance.Editor_SetUsableType(consumableAsset);
                if (type == "1")
                {
                    instance.Editor_SetUsableType(keyAsset);
                }

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