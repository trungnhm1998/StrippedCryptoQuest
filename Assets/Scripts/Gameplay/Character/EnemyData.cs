using System;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Loot;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEditor.Localization.Plugins.XLIFF.V20;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace CryptoQuest.Gameplay.Character
{
    [Serializable]
    public struct Drop
    {
        public float Chance;
        [SerializeReference] public LootInfo LootItem;
    }

    [Serializable]
    public class EnemyInfomation : CharacterInformation
    {
        [field: SerializeField, ReadOnly] public string DisplayName { get; private set; }

        private EnemyData _data;
        public new EnemyData Data => _data;

        public EnemyInfomation(EnemyData enemyData) : base(enemyData)
        {
            _data = enemyData;
            Element = _data.Element;
        }

        /// <summary>
        // To separate with other same Enemy in a group
        // their name will be post fix with A, B, C, D 
        /// </summary>
        /// <param name="postFix"></param>
        public void SetDisplayName(string postFix)
        {
            DisplayName = $"{Data.Name.GetLocalizedString()}{postFix}";
        }
    }

    /// <summary>
    /// Enemy structure (https://docs.google.com/spreadsheets/d/1WkX1DyDOGf6EiAppo8Buz2sUkSKV5OnDENEvmHzKXNQ/edit#gid=1024080951)
    /// </summary>
    [CreateAssetMenu(menuName = "Create EnemyData", fileName = "EnemyData", order = 0)]
    public class EnemyData : CharacterData
    {
        // This is not experience of the enemy, it's exp player gain after defeat this enemy
        [field: SerializeField] public int Exp { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }
        [field: SerializeField] public Elemental Element { get; private set; }

        [field: SerializeField] public AttributeWithValue[] Stats { get; private set; } =
            Array.Empty<AttributeWithValue>();

        [SerializeField] private Drop[] _drops = Array.Empty<Drop>();
        public Drop[] Drops => _drops;

        public new EnemyInfomation CreateCharacterInfo()
        {
            return new EnemyInfomation(this);
        }

#if UNITY_EDITOR
        public void Editor_AddDrop(EquipmentInfo equipment)
        {
            ArrayUtility.Add(ref _drops, new Drop()
            {
                LootItem = new EquipmentLootInfo(equipment),
                Chance = 1
            });
        }

        public void Editor_AddDrop(UsableInfo consumable)
        {
            ArrayUtility.Add(ref _drops, new Drop()
            {
                LootItem = new UsableLootInfo(consumable),
                Chance = 1
            });
        }

        public void Editor_AddDrop(CurrencyInfo currencyInfo)
        {
            ArrayUtility.Add(ref _drops, new Drop()
            {
                LootItem = new CurrencyLootInfo(currencyInfo),
                Chance = 1
            });
        }

        public void Editor_SetMonsterPrefab(GameObject prefab)
        {
            Prefab = prefab;
        }
#endif
    }
}