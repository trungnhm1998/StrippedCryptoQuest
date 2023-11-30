using System;
using System.IO;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Item;
using CryptoQuest.Item.Consumable;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Inventory
{
    public class UsableSOBrowserEditor : ScriptableObjectBrowserEditor<ConsumableSO>
    {
        private const string DEFAULT_NAME = "Usable";
        private const int ROW_OFFSET = 2;
        private const int ROW_ITEM_ID = 0;
        private const int ROW_ITEM_TYPE = 7;

        private const string ITEM_TYPE = "1";
        private const string DATA_PATH = "Assets/ScriptableObjects/Data/Inventory/UsableTypes/";

        public UsableSOBrowserEditor()
        {
            CreateDataFolder = false;

            DefaultStoragePath = "Assets/ScriptableObjects/Data/Inventory/Items/Usables";
        }

        /// <summary>
        /// <see cref="ConsumableSO.Editor_SetUsableType"/>
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="callback"></param>
        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] rows = File.ReadAllLines(directory);

            for (int index = ROW_OFFSET; index < rows.Length; index++)
            {
                // get data form tsv file
                string[] cols = rows[index].Split('\t');
                string id = cols[ROW_ITEM_ID];
                string name = DEFAULT_NAME + id;
                string type = cols[ROW_ITEM_TYPE];
                string path = DefaultStoragePath + "/" + name + ".asset";

                ConsumableSO instance = null;

                // find instance if null create new
                instance = (ConsumableSO)AssetDatabase.LoadAssetAtPath(path, typeof(ConsumableSO));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<ConsumableSO>();
                }

                // import Data
                instance.Editor_SetID(id);
                instance.name = name;

                instance.Editor_SetUsableType(EConsumableType.Consumable);
                if (type == ITEM_TYPE)
                {
                    instance.Editor_SetUsableType(EConsumableType.Key);
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