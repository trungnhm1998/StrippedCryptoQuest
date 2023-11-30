using System;
using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Character.Beast
{
    public interface IBeast
    {
        public PassiveAbility Passive { get; }
        LocalizedString LocalizedName { get; }
        Elemental Elemental { get; }
        string Name { get; }
    }

    [Serializable]
    public class Beast : IBeast
    {
        [field: SerializeField] public int Id { get; set; }
        [field: SerializeField] public int Level { get; set; }
        [field: SerializeField] public int TokenId { get; set; }
        [field: SerializeField] public Elemental Elemental { get; set; }
        public string Name => Type.BeastInformation.LocalizedName.GetLocalizedString();
        [field: SerializeField] public CharacterClass Class { get; set; }
        [field: SerializeField] public BeastTypeSO Type { get; set; }
        [field: SerializeField] public PassiveAbility Passive { get; set; }
        public LocalizedString LocalizedName => Type.BeastInformation.LocalizedName;
    }
}