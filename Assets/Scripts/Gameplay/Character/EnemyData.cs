using System;
using CryptoQuest.Gameplay.Inventory.Items;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CryptoQuest.Gameplay.Character
{
    [Serializable]
    public struct Drop
    {
        public float Chance;
        [SerializeReference] public ItemInfo Item;
    }

    [CreateAssetMenu(menuName = "Create EnemyData", fileName = "EnemyData", order = 0)]
    public class EnemyData : CharacterData
    {
        [field: SerializeField] public int Exp { get; private set; }
        [field: SerializeField] public AssetReference Prefab { get; private set; }

        [field: SerializeField] public AttributeWithValue[] Stats { get; private set; } =
            Array.Empty<AttributeWithValue>();

        [SerializeField] private Drop[] _drops = Array.Empty<Drop>();
        public Drop[] Drops => _drops;

#if UNITY_EDITOR
        public void Editor_AddDrop(EquipmentInfo equipment)
        {
            ArrayUtility.Add(ref _drops, new Drop()
            {
                Item = equipment,
                Chance = 1
            });
        }

        public void Editor_AddDrop(UsableInfo consumable)
        {
            ArrayUtility.Add(ref _drops, new Drop()
            {
                Item = consumable,
                Chance = 1
            });
        }

        public void Editor_SetMonsterPrefab(AssetReference prefab)
        {
            Prefab = prefab;
        }
#endif
    }
}