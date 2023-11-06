using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuestEditor.Helper;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CryptoQuestEditor
{
    [CustomEditor(typeof(InventorySO))]
    public class InventorySOEditor : Editor
    {
        private const string TOOLBAR_MASTER_DATA = "Select Master Data";

        private const int ROW_HEADER = 0;
        private const int ROW_OFFSET = 2;
        private const int ROW_FOREIGN_KEY = 1;
        private const int ROW_RARITY_ID = 16;
        private const int ROW_MAX_LEVEL = 23;
        private const int ROW_IS_NFT_ITEM = 54;

        private const string EXTENSION_TYPE = "tsv";
        private const string MIN_ATTRIBUTE = "min";
        private const string MAX_ATTRIBUTE = "max";
        private const string IS_NFT_ITEM = "1";

        private readonly Dictionary<string, AttributeScriptableObject> _AttributeSOCached = new();

        [SerializeField] private VisualTreeAsset _uxml;
        private Button _equipmentButton;
        private Button _masterDataButton;
        private Button _usableItemButton;
        private Button _removeButton;
        private InventorySO Target => target as InventorySO;

        #region Attribute Path

        private const string DEFAULT_ATTRIBUTE_PATH = "Assets/ScriptableObjects/Battle/Characters/Attributes";

        private const string AGILITY_ATTRIBUTE_FILE = "/Default.Agility.asset";
        private const string ATTACK_ATTRIBUTE_FILE = "/Default.CriticalRate.asset";
        private const string CRIT_ATTRIBUTE_FILE = "/Default.Crit.asset";
        private const string CRITICAL_RATE_ATTRIBUTE_FILE = "/Default.CriticalRate.asset";
        private const string DEFENSE_ATTRIBUTE_FILE = "/Default.Defense.asset";
        private const string EVASION_RATE_ATTRIBUTE_FILE = "/Default.EvasionRate.asset";
        private const string EXPERIENCE_POINTS_ATTRIBUTE_FILE = "/Default.ExperiencePoints.asset";
        private const string HP_ATTRIBUTE_FILE = "/Default.HP.asset";
        private const string INTELLIGENCE_ATTRIBUTE_FILE = "/Default.Intelligence.asset";
        private const string LUCK_ATTRIBUTE_FILE = "/Default.Luck.asset";
        private const string MAGIC_ATTACK_ATTRIBUTE_FILE = "/Default.MagicAttack.asset";
        private const string MAX_HP_ATTRIBUTE_FILE = "/Default.MaxHP.asset";
        private const string MAX_MP_ATTRIBUTE_FILE = "/Default.MaxMP.asset";
        private const string MP_ATTRIBUTE_FILE = "/Default.MP.asset";
        private const string SKILL_POWER_ATTRIBUTE_FILE = "/Default.SkillPower.asset";
        private const string STRENGTH_ATTRIBUTE_FILE = "/Default.Strength.asset";
        private const string VITALITY_ATTRIBUTE_FILE = "/Default.Vitality.asset";

        private readonly Dictionary<string, string> _attributeMap = new()
        {
            { "agility", DEFAULT_ATTRIBUTE_PATH + AGILITY_ATTRIBUTE_FILE },
            { "attack", DEFAULT_ATTRIBUTE_PATH + ATTACK_ATTRIBUTE_FILE },
            { "crit", DEFAULT_ATTRIBUTE_PATH + CRIT_ATTRIBUTE_FILE },
            { "critical_rate", DEFAULT_ATTRIBUTE_PATH + CRITICAL_RATE_ATTRIBUTE_FILE },
            { "defense", DEFAULT_ATTRIBUTE_PATH + DEFENSE_ATTRIBUTE_FILE },
            { "evasion_rate", DEFAULT_ATTRIBUTE_PATH + EVASION_RATE_ATTRIBUTE_FILE },
            { "experience_points", DEFAULT_ATTRIBUTE_PATH + EXPERIENCE_POINTS_ATTRIBUTE_FILE },
            { "hp", DEFAULT_ATTRIBUTE_PATH + HP_ATTRIBUTE_FILE },
            { "intelligence", DEFAULT_ATTRIBUTE_PATH + INTELLIGENCE_ATTRIBUTE_FILE },
            { "luck", DEFAULT_ATTRIBUTE_PATH + LUCK_ATTRIBUTE_FILE },
            { "magic_attack", DEFAULT_ATTRIBUTE_PATH + MAGIC_ATTACK_ATTRIBUTE_FILE },
            { "max_hp", DEFAULT_ATTRIBUTE_PATH + MAX_HP_ATTRIBUTE_FILE },
            { "max_mp", DEFAULT_ATTRIBUTE_PATH + MAX_MP_ATTRIBUTE_FILE },
            { "mp", DEFAULT_ATTRIBUTE_PATH + MP_ATTRIBUTE_FILE },
            { "skill_power", DEFAULT_ATTRIBUTE_PATH + SKILL_POWER_ATTRIBUTE_FILE },
            { "strength", DEFAULT_ATTRIBUTE_PATH + STRENGTH_ATTRIBUTE_FILE },
            { "vitality", DEFAULT_ATTRIBUTE_PATH + VITALITY_ATTRIBUTE_FILE },
        };

        #endregion

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            _uxml.CloneTree(root);

            _equipmentButton = root.Q<Button>("add-equipment-button");
            _masterDataButton = root.Q<Button>("add-master-data-button");
            _usableItemButton = root.Q<Button>("add-consumable-button");
            _removeButton = root.Q<Button>("remove-all-button");

            _equipmentButton.clicked += AddAllEquipment;
            _masterDataButton.clicked += AddMasterData;
            _usableItemButton.clicked += AddAllUsableItem;
            _removeButton.clicked += RemoveAll;

            return root;
        }

        private void AddAllEquipment()
        {
            // var allEquipment = ToolsHelper.GetAssets<EquipmentDef>();
            //
            // foreach (var equipment in allEquipment)
            // {
            //     Target.Editor_Add(new EquipmentInfo(equipment.ID));
            // }
            //
            // EditorUtility.SetDirty(Target);
            // AssetDatabase.SaveAssets();
        }

        private void AddAllUsableItem()
        {
            ConsumableSO[] allUsableItem = ToolsHelper.GetAssets<ConsumableSO>();

            foreach (ConsumableSO usableItem in allUsableItem)
            {
                Target.Consumables.Add(new ConsumableInfo(usableItem));
            }
        }

        private void RemoveAll()
        {
            ConsumableInfo[] usableItems = Target.Consumables.ToArray();
            EquipmentInfo[] equipmentItems = Target.Equipments.ToArray();

            foreach (var item in usableItems)
            {
                Target.Consumables.RemoveAll(info => info == item);
            }

            foreach (var item in equipmentItems)
            {
                Target.Equipments.RemoveAll(info => info == item);
            }
        }

        private void AddMasterData()
        {
            string path = EditorUtility.OpenFilePanel(TOOLBAR_MASTER_DATA, "", EXTENSION_TYPE);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning($"No file selected");
                return;
            }

            string[] rows = File.ReadAllLines(path);
            string[] headerFields = rows[ROW_HEADER].Split('\t');

            var equipments = ToolsHelper.GetAssets<EquipmentDef>();

            Dictionary<string, EquipmentDef> lookupTable =
                equipments.ToDictionary(equipment => equipment.ID, value => value);

            // for (int index = ROW_OFFSET; index < rows.Length; index++)
            // {
            //     string[] cols = rows[index].Split('\t');
            //     string id = cols[ROW_FOREIGN_KEY];
            //     if (!lookupTable.TryGetValue(id, out var equipment)) continue;
            //
            //     var instance = new EquipmentInfo(equipment.ID);
            //     FillEquipmentData(cols, instance, headerFields);
            //     Target.Editor_Add(instance);
            // }
        }

        private void FillEquipmentData(string[] cols, EquipmentInfo instance, string[] headerFields)
        {
            int maxLevelEquipment = Convert.ToInt32(cols[ROW_MAX_LEVEL]);

            // instance.Editor_SetRarity(GetRarity(ParseData(cols[ROW_RARITY_ID])));
            // instance.Editor_SetIsNftItem(IsNftItem(cols[ROW_IS_NFT_ITEM]));

            SetEquipmentStats(cols, headerFields, instance, maxLevelEquipment);
        }

        private void SetEquipmentStats(string[] cols, string[] headerFields, EquipmentInfo instance,
            int maxLevelEquipment)
        {
            Dictionary<AttributeScriptableObject, CappedAttributeDef> attributes = new();

            for (int i = 0; i < cols.Length; i++)
            {
                string data = cols[i];
                string header = headerFields[i];
                string[] attributeConfig = header.Split("_");

                if (string.IsNullOrEmpty(data)) continue;
                if (!(header.Contains(MIN_ATTRIBUTE) || header.Contains(MAX_ATTRIBUTE))) continue;

                string type = attributeConfig[0];
                string attributeName = attributeConfig[1];

                AttributeScriptableObject attribute = GetAttribute(attributeName);
                if (attribute == null) continue;

                attributes.TryAdd(attribute, new CappedAttributeDef(attribute: attribute));
                CappedAttributeDef attributeDef = attributes[attribute];

                switch (type)
                {
                    case MIN_ATTRIBUTE:
                        attributeDef.MinValue = float.Parse(data);
                        break;
                    case MAX_ATTRIBUTE:
                        attributeDef.MaxValue = float.Parse(data);
                        break;
                }

                attributes[attribute] = attributeDef;
            }

            StatsDef stats = new();
            stats.Attributes = attributes.Values.ToArray();
            stats.MaxLevel = maxLevelEquipment;

            // instance.Editor_SetStats(stats);
        }


        private AttributeScriptableObject GetAttribute(string attributeName)
        {
            if (_AttributeSOCached.TryGetValue(attributeName, out var attribute)) return attribute;
            if (_attributeMap.TryGetValue(attributeName, out var attributePath) == false) return null;

            attribute = (AttributeScriptableObject)AssetDatabase.LoadAssetAtPath(attributePath,
                typeof(AttributeScriptableObject));

            _AttributeSOCached.Add(attributeName, attribute);

            return attribute;
        }

        private RaritySO GetRarity(int id)
        {
            RaritySO[] allRarities = ToolsHelper.GetAssets<RaritySO>();

            return allRarities.FirstOrDefault(rarity => rarity.ID == id);
        }

        private bool IsNftItem(string data) => data == IS_NFT_ITEM;
        private int ParseData(string data) => string.IsNullOrEmpty(data) ? 0 : int.Parse(data);
    }
}