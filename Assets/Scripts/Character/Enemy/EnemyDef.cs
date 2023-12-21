using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.Character.Enemy
{
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

        [field: SerializeField] private Drop[] _drops { get; set; } = Array.Empty<Drop>();

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
    }
}