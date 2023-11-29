using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Gameplay;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Character.Beast
{
    [Serializable]
    public class BeastSpec
    {
        [field: SerializeField] public Elemental Elemental { get; set; }
        [field: SerializeField] public CharacterClass Class { get; set; }
        [field: SerializeField] public PassiveAbility[] Passives { get; set; } = Array.Empty<PassiveAbility>();
        [field: SerializeField] public BeastTypeSO BeastTypeSo { get; set; }
    }
}