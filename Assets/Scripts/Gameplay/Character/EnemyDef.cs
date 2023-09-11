using System;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Loot;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CryptoQuest.Gameplay.Character
{
    [Serializable]
    public struct Drop
    {
        public float Chance;
        [SerializeReference] public LootInfo LootItem;
    }

    /// <summary>
    /// Enemy structure (https://docs.google.com/spreadsheets/d/1WkX1DyDOGf6EiAppo8Buz2sUkSKV5OnDENEvmHzKXNQ/edit#gid=1024080951)
    /// </summary>
    [CreateAssetMenu(menuName = "Create EnemyData", fileName = "EnemyData", order = 0)]
    public class EnemyDef : CharacterData<EnemyDef, EnemySpec>
    {
        // This is not experience of the enemy, it's exp player gain after defeat this enemy
        [field: SerializeField] public int Exp { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }

        [field: SerializeField] public AttributeWithValue[] Stats { get; private set; } =
            Array.Empty<AttributeWithValue>();

        [SerializeField] private Drop[] _drops = Array.Empty<Drop>();
        public Drop[] Drops => _drops;

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