using System;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Loot;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
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

    [CreateAssetMenu(menuName = "Create EnemyData", fileName = "EnemyData", order = 0)]
    public class EnemyData : CharacterData
    {
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

        public void Editor_SetEXP(int exp)
        {
            Exp = exp;
        }

        public void Editor_SetStats(AttributeWithValue[] stats)
        {
            Stats = stats;
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

        public void Editor_ClearDrop()
        {
            _drops = Array.Empty<Drop>();
        }

        public void Editor_SetMonsterPrefab(GameObject prefab)
        {
            Prefab = prefab;
        }
#endif
    }
}