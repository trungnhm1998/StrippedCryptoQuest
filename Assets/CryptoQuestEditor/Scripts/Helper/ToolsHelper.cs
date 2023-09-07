using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Helper
{
    public static class ToolsHelper
    {
        /// <summary>
        /// This method is used to get all assets from a type.
        /// </summary>
        ///
        /// <example>
        /// Find all apple in a fruit shop.
        /// </example>
        ///
        /// <typeparam name="T">The type assets want to find</typeparam>
        /// <returns></returns>
        public static T[] GetAssets<T>() where T : Object
        {
            List<T> assetList = new List<T>();
            string typeName = typeof(T).Name;

            foreach (string guid in AssetDatabase.FindAssets($"t:{typeName}"))
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Object[] assetsAtPath = AssetDatabase.LoadAllAssetsAtPath(assetPath);

                foreach (Object asset in assetsAtPath)
                {
                    if (asset is T typedAsset)
                    {
                        assetList.Add(typedAsset);
                    }
                }
            }

            T[] assets = assetList.ToArray();
            return assets;
        }

        /// <summary>
        /// This method is used to get an asset from a name.
        /// </summary>
        ///
        /// <example>
        /// Find an apple in a fruit shop. 
        /// </example>
        /// 
        /// <param name="value">The name asset want to find </param>
        /// <typeparam name="T">The type asset want to find</typeparam>
        /// <returns></returns>
        public static T GetAsset<T>(string value) where T : Object
        {
            string typeName = $"{typeof(T).Name} {value}";
            string[] paths = AssetDatabase.FindAssets($"t:{typeName} ");

            if (paths.Length <= 0 || value == "") return null;

            string assetPath = AssetDatabase.GUIDToAssetPath(paths[0]);
            T asset = (T)AssetDatabase.LoadAssetAtPath(assetPath, typeof(T));

            return asset;
        }
    }
}