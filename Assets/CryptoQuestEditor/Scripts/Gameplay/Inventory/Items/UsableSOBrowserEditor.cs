using System;
using System.IO;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using IndiGames.Tools.ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Inventory
{
    public class UsableSOBrowserEditor : ScriptableObjectBrowserEditor<UsableSO>
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
        /// <see cref="UsableSO.Editor_SetUsableType"/>
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

                UsableSO instance = null;

                // find instance if null create new
                instance = (UsableSO)AssetDatabase.LoadAssetAtPath(path, typeof(UsableSO));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<UsableSO>();
                }

                var keyAsset =
                    (UsableTypeSO)AssetDatabase.LoadAssetAtPath(
                        $"{DATA_PATH}KeyItem.asset", typeof(UsableTypeSO));
                var consumableAsset =
                    (UsableTypeSO)AssetDatabase.LoadAssetAtPath(
                        $"{DATA_PATH}Consumable.asset", typeof(UsableTypeSO));

                // import Data
                instance.Editor_SetID(id);
                instance.name = name;

                instance.Editor_SetUsableType(consumableAsset);
                if (type == ITEM_TYPE)
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