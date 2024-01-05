using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Helper;
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

        [SerializeField] private EquipmentPrefabDatabase _prefabDatabase;
        [SerializeField] private string _exportPath = "Assets/ScriptableObjects/Equipments";
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

                    var prefabId = csv.GetField<string>("equipment_id_foreign");
                    var equipmentType = LookupEquipmentType[csv.GetField<int>("equip_type_id")];
                    var categoryName = equipmentType.EquipmentCategory.ToString();
                    var categoryPath = $"{_exportPath}/{categoryName}";
                    var equipmentPath = $"{categoryPath}/{equipmentData.ID}.asset";

                    var stats = new List<AttributeWithValue>();
                    foreach (var attributeMap in _attributeMap)
                    {
                        var stat = ConvertAttribute(attributeMap, csv, equipmentData);
                        if (stat.Attribute != null) stats.Add(stat);
                    }
                    equipmentData.Stats = stats.ToArray();
                        
                    var passives = new List<PassiveAbility>();
                    passives.AddRange(FindAndSetSkill(csv, "condition_skill_id"));
                    passives.AddRange(FindAndSetSkill(csv, "passive_skill_id_1"));
                    passives.AddRange(FindAndSetSkill(csv, "passive_skill_id_2"));
                    equipmentData.Passives = passives.ToArray();
                    
                    // check path exist
                    CreateFolderIfNotExist(_exportPath);
                    CreateFolderIfNotExist(categoryPath);

                    var equipment = AssetDatabase.LoadAssetAtPath<EquipmentSO>(equipmentPath);
                    if (equipment == null)
                    {
                        equipment = CreateInstance<EquipmentSO>();
                        AssetDatabase.CreateAsset(equipment, equipmentPath);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.ImportAsset(equipmentPath);
                    }

                    var equipmentSO = new SerializedObject(equipment);
                    equipmentSO.FindProperty("<Data>k__BackingField").boxedValue = equipmentData;
                    equipmentSO.ApplyModifiedProperties();
                    equipmentSO.Update();

                    _prefabDatabase.LoadDataByIdAsync(prefabId).Completed += handle =>
                    {
                        equipmentData.Prefab = handle.Result;
                        equipmentSO.FindProperty("<Data>k__BackingField").boxedValue = equipmentData;
                        equipmentSO.ApplyModifiedProperties();
                        equipmentSO.Update();
                    };


                    equipmentSO.ApplyModifiedProperties();
                    equipmentSO.Update();
                }
            }
        }

        private ILevelAttributeCalculator _levelCalculator = new DefaultLevelAttributeCalculator();
        private AttributeWithValue ConvertAttribute(NameAttributeMap attributeMap, CsvReader csv, EquipmentData equipment)
        {
            csv.TryGetField<int>($"min_{attributeMap.Name}", out var min);
            csv.TryGetField<int>($"max_{attributeMap.Name}", out var max);
            if (min <= 0 || max <= 0) return new AttributeWithValue();
            var cappedAttribute = new CappedAttributeDef(attributeMap.Attribute);
            cappedAttribute.MinValue = min;
            cappedAttribute.MaxValue = max;
            return new AttributeWithValue(attributeMap.Attribute,
                _levelCalculator.GetValueAtLevel(equipment.MinLevel, cappedAttribute, equipment.MaxLevel));
        }


        private IEnumerable<PassiveAbility> FindAndSetSkill(CsvReader csv, string fieldName)
        {
            csv.TryGetField<int>(fieldName, out var skillId);
            if (skillId <= 0) yield break;
            // load all asset of type skillType
            var guids = AssetDatabase.FindAssets($"t:{typeof(PassiveAbility).Name}");
            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<PassiveAbility>(assetPath);
                if (asset.Id == skillId)
                    yield return asset;
            }
        }

        private static void CreateFolderIfNotExist(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder(Path.GetDirectoryName(path), Path.GetFileName(path));
        }
    }
}