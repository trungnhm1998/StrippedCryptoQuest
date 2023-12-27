using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Character;
using CryptoQuest.Gameplay;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Beast
{
    public interface IBeast
    {
        public PassiveAbility Passive { get; }
        LocalizedString LocalizedName { get; }
        Elemental Elemental { get; }
        CharacterClass Class { get; }
        string BeastId { get; }
        int Id { get; }
        BeastTypeSO Type { get; }
        StatsDef Stats { get; }
        int Level { get; }
        int MaxLevel { get; }
        int Stars { get; }
        bool IsValid();
    }

    [Serializable]
    public class Beast : IBeast
    {
        [field: SerializeField] public int Id { get; set; }
        [field: SerializeField] public string BeastId { get; set; }
        [field: SerializeField] public int Level { get; set; }
        [field: SerializeField] public int MaxLevel { get; set; }
        [field: SerializeField] public int Stars { get; set; }
        [field: SerializeField] public int TokenId { get; set; }
        [field: SerializeField] public Elemental Elemental { get; set; }
        [field: SerializeField] public CharacterClass Class { get; set; }
        [field: SerializeField] public BeastTypeSO Type { get; set; }
        [field: SerializeField] public StatsDef Stats { get; set; }
        [field: SerializeField] public PassiveAbility Passive { get; set; }
        public Sprite Image { get; set; }
        public LocalizedString LocalizedName => Type.BeastInformation.LocalizedName;
        public bool IsValid() => Elemental != null && Class != null && Type != null;
    }
}