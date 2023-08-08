using System;
using System.IO;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using ScriptableObjectBrowser;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Gameplay.Inventory.Items
{
    [CustomEditor(typeof(WeaponSO))]
    public class WeaponScriptableObjectEditor : Editor
    {
        /// <summary>
        /// <see cref="CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.WeaponSO.Editor_SetEquipmentType"/> 
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var weapon = (WeaponSO)target;
            if (!(weapon.EquipmentType is WeaponTypeSO))
            {
                weapon.Editor_SetEquipmentType(null);
            }
        }
    }

    public class WeaponSOEditor : ScriptableObjectBrowserEditor<WeaponSO>
    {
        private const int ROW_OFFSET = 2;

        public WeaponSOEditor()
        {
            this.createDataFolder = false;

            this.defaultStoragePath = "Assets/ScriptableObjects/Data/Inventory/Items/Equipments/Weapons";
        }

        /// <summary>
        /// For Equipment Type  <see cref="CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.WeaponSO.Editor_SetEquipmentType"/>
        /// For Rarity  <see cref="CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.EquipmentSO.Editor_SetRarity"/>
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="callback"></param>
        public override void ImportBatchData(string directory, Action<ScriptableObject> callback)
        {
            string[] allLines = File.ReadAllLines(directory);

            for (var index = ROW_OFFSET; index < allLines.Length; index++)
            {
                // get data form tsv file
                string[] splitedData = allLines[index].Split('\t');
                var id = splitedData[0];
                var type = splitedData[6].Replace(" ", "_");
                var name = type + id;
                var rarity = splitedData[7];
                var path = this.defaultStoragePath + "/" + name + ".asset";

                WeaponSO instance = null;

                // find instance if null create new
                instance = (WeaponSO)AssetDatabase.LoadAssetAtPath(path, typeof(WeaponSO));
                if (instance == null || !AssetDatabase.Contains(instance))
                {
                    instance = ScriptableObject.CreateInstance<WeaponSO>();
                }

                WeaponTypeSO weaponType = (WeaponTypeSO)AssetDatabase.LoadAssetAtPath(
                    "Assets/ScriptableObjects/Data/Inventory/ItemTypes/Equipments/" + type + ".asset",
                    typeof(WeaponTypeSO));

                RaritySO raritySO = (RaritySO)AssetDatabase.LoadAssetAtPath(
                    "Assets/ScriptableObjects/Data/Inventory/RarityTypes/" + rarity + ".asset",
                    typeof(RaritySO));

                // import Data
                instance.ID = id;
                instance.name = name;
                instance.Editor_SetEquipmentType(weaponType);
                instance.Editor_SetRarity(raritySO);

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