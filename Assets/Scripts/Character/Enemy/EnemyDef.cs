using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Gameplay.Loot;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

    [Serializable]
    public struct Skills
    {
        public float Probability;
        public CastSkillAbility SkillDef;
    }

    /// <summary>
    /// Enemy structure (https://docs.google.com/spreadsheets/d/1WkX1DyDOGf6EiAppo8Buz2sUkSKV5OnDENEvmHzKXNQ/edit#gid=1024080951)
    /// </summary>
    [Serializable]
    public class EnemyDef : ScriptableObject
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public LocalizedString Name { get; private set; }
        [field: SerializeField] public Elemental Element { get; private set; }
        [field: SerializeField] public AssetReferenceT<GameObject> Model { get; private set; }

        [field: SerializeField]
        public AttributeWithValue[] Stats { get; private set; } =
            Array.Empty<AttributeWithValue>();

        [SerializeField] private Drop[] _drops = Array.Empty<Drop>();
        public Drop[] Drops => _drops;

        [SerializeField] private float _normalAttackProbability = 1f;
        public float NormalAttackProbability => _normalAttackProbability;
        [SerializeField] private Skills[] _skills = Array.Empty<Skills>();
        public Skills[] Skills => _skills;

        /// <summary>
        /// Factory method
        /// Need to create character info when there're
        /// many characters using the same data set, like enemies
        /// </summary>
        /// <returns>New spec to use at runtime for this enemy</returns>
        public EnemySpec CreateCharacterSpec()
        {
            var character = new EnemySpec();
            character.Init(this);
            return character;
        }

#if UNITY_EDITOR
        public void Editor_AddDrop(LootInfo loot)
        {
            ArrayUtility.Add(ref _drops, new Drop()
            {
                LootItem = loot,
                Chance = 1
            });
        }

        public void Editor_AddDrop(LootInfo loot, float chance)
        {
            ArrayUtility.Add(ref _drops, new Drop()
            {
                LootItem = loot,
                Chance = chance
            });
        }

        public void Editor_SetStats(AttributeWithValue[] stats)
        {
            Stats = stats;
        }

        public void Editor_SetNameKey(LocalizedString name)
        {
            Name = name;
        }

        public void Editor_ClearDrop()
        {
            _drops = Array.Empty<Drop>();
        }

        public void Editor_SetMonsterModelAssetRef(AssetReferenceT<GameObject> assetReference)
        {
            Model = assetReference;
        }

        public void Editor_SetElement(Elemental element)
        {
            Element = element;
        }
#endif
    }
}