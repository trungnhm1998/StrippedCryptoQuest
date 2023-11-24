using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Gameplay.Loot;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace CryptoQuest.Character.Enemy
{
    [Serializable]
    public struct Drop
    {
        public float Chance;

        [field: SerializeReference, SubclassSelector, FormerlySerializedAs("LootItem")]
        public LootInfo LootItem { get; set; }

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

        public Drop[] Drops
        {
            get => _drops;
            set => _drops = value;
        }

        [SerializeReference, SubclassSelector] private StealableInfo[] _stealableInfos
            = Array.Empty<StealableInfo>();

        public StealableInfo[] StealableInfos => _stealableInfos;

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
    }
}