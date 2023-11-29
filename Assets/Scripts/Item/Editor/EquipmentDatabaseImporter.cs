using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CsvHelper;
using CsvHelper.Configuration;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace CryptoQuestEditor.Item
{
    public class EquipmentDatabaseImporter : ScriptableObject
    {
        private sealed class EquipmentDataMap : ClassMap<EquipmentData>
        {
            public EquipmentDataMap()
            {
                Map(m => m.ID).Name("equipment_id");
                // Map(m => m.PrefabId).Name("equipment_id_foreign");
                Map(m => m.Stars).Name("star");
                Map(m => m.RequiredCharacterLevel).Name("required_lv");
                Map(m => m.MinLevel).Name("min_lv");
                Map(m => m.MaxLevel).Name("max_lv");
                Map(m => m.ValuePerLvl).Name("value_per_lv");
            }
        }

        [Serializable]
        public class NameAttributeMap
        {
            public string Name;
            public AttributeScriptableObject Attribute;
        }

        [SerializeField] private string _exportPath = "Assets/ScriptableObjects/Equipments";
        [SerializeField] private EquipmentDatabase _database;
        [SerializeField] private TextAsset[] _csvFiles;

        [Header("Mapping asset")]
        [SerializeField] private EquipmentTypeSO[] _equipmentTypes;

        [SerializeField] private RaritySO[] _rarities;
        [SerializeField] private NameAttributeMap[] _attributeMap;

        private Dictionary<int, EquipmentTypeSO> _lookupEquipmentType = default;
        private Dictionary<string, AttributeScriptableObject> _lookupAttribute = default;
        private Dictionary<int, RaritySO> _lookupRarity = default;

        private Dictionary<int, EquipmentTypeSO> LookupEquipmentType
        {
            get
            {
                if (_lookupEquipmentType != null && _lookupEquipmentType.Count > 0) return _lookupEquipmentType;
                _lookupEquipmentType = new Dictionary<int, EquipmentTypeSO>();
                foreach (var equipmentType in _equipmentTypes)
                    _lookupEquipmentType.Add(equipmentType.Id, equipmentType);
                return _lookupEquipmentType;
            }
        }

        private Dictionary<string, AttributeScriptableObject> LookupAttribute
        {
            get
            {
                if (_lookupAttribute != null && _lookupAttribute.Count > 0) return _lookupAttribute;
                _lookupAttribute = new Dictionary<string, AttributeScriptableObject>();
                foreach (var map in _attributeMap) _lookupAttribute.Add(map.Name, map.Attribute);
                return _lookupAttribute;
            }
        }

        private Dictionary<int, RaritySO> LookupRarity
        {
            get
            {
                if (_lookupRarity != null && _lookupRarity.Count > 0) return _lookupRarity;
                _lookupRarity = new Dictionary<int, RaritySO>();
                foreach (var rarity in _rarities) _lookupRarity.Add(rarity.ID, rarity);
                return _lookupRarity;
            }
        }

        [ContextMenu("Import")]
        public void Import()
        {
            foreach (var csvFile in _csvFiles)
            {
                var csvPath = AssetDatabase.GetAssetPath(csvFile);
                using var fs = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var stream = new StreamReader(fs);

                var config = new CsvConfiguration(CultureInfo.InvariantCulture);
                using var csv = new CsvReader(stream, config);
                csv.Context.RegisterClassMap<EquipmentDataMap>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var equipmentData = csv.GetRecord<EquipmentData>();
                    equipmentData.Rarity = LookupRarity[csv.GetField<int>("rarity_id")];
                    
                    EquipmentDefSO equipmentDef;
                    var equipmentType = LookupEquipmentType[csv.GetField<int>("equip_type_id")];
                    var categoryName = equipmentType.EquipmentCategory.ToString();
                    var categoryPath = $"{_exportPath}/{categoryName}";
                    var equipmentPath = $"{categoryPath}/{equipmentData.ID}.asset";
                    // check path exist
                    if (!AssetDatabase.IsValidFolder(categoryPath))
                        AssetDatabase.CreateFolder(_exportPath, categoryName);
                    equipmentDef = AssetDatabase.LoadAssetAtPath<EquipmentDefSO>(equipmentPath);
                    if (equipmentDef == null)
                    {
                        equipmentDef = CreateInstance<EquipmentDefSO>();
                        AssetDatabase.CreateAsset(equipmentDef, equipmentPath);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.ImportAsset(equipmentPath);
                    }

                    var equipmentSO = new SerializedObject(equipmentDef);
                    equipmentSO.FindProperty("<Data>k__BackingField").boxedValue = equipmentData;
                    equipmentSO.ApplyModifiedProperties();
                    equipmentSO.Update();
                }
            }
        }
    }
}