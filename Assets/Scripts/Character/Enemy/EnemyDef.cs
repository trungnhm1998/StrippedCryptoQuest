using System;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Loot;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Character.Enemy
{
    [Serializable]
    public struct Drop
    {
        public float Chance;
        [SerializeReference] public LootInfo LootItem;

        /// <returns>a cloned of loot config</returns>
        public LootInfo CreateLoot() => LootItem.Clone();
    }

    /// <summary>
    /// Enemy structure (https://docs.google.com/spreadsheets/d/1WkX1DyDOGf6EiAppo8Buz2sUkSKV5OnDENEvmHzKXNQ/edit#gid=1024080951)
    /// </summary>
    [CreateAssetMenu(menuName = "Create EnemyData", fileName = "EnemyData", order = 0)]
    public class EnemyDef : CharacterData<EnemyDef, EnemySpec>
    {
        [field: SerializeField] public LocalizedString Name { get; private set; }
        [field: SerializeField] public LocalizedString Description { get; private set; }
        [field: SerializeField] public Elemental Element { get; private set; }

        [field: SerializeField] public GameObject Prefab { get; private set; }

        [field: SerializeField] public AttributeWithValue[] Stats { get; private set; } =
            Array.Empty<AttributeWithValue>();

        [SerializeField] private Drop[] _drops = Array.Empty<Drop>();
        public Drop[] Drops => _drops;

#if UNITY_EDITOR
        public void Editor_AddDrop(LootInfo loot)
        {
            ArrayUtility.Add(ref _drops, new Drop()
            {
                LootItem = loot,
                Chance = 1
            });
        }

        public void Editor_SetStats(AttributeWithValue[] stats)
        {
            Stats = stats;
        }

        public void Editor_ClearDrop()
        {
            _drops = Array.Empty<Drop>();
        }

        public void Editor_SetMonsterPrefab(GameObject prefab)
        {
            Prefab = prefab;
        }

        public void Editor_SetElement(Elemental element)
        {
            Element = element;
        }
#endif
    }
}