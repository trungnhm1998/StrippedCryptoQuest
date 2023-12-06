using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Character;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Beast.ScriptableObjects
{
    public class NullBeast : IBeast
    {
        private static NullBeast _instance;
        public static NullBeast Instance => _instance ??= new NullBeast();
        public PassiveAbility Passive => ScriptableObject.CreateInstance<PassiveAbility>();
        public LocalizedString LocalizedName => new();
        public Elemental Elemental => ScriptableObject.CreateInstance<Elemental>();
        public CharacterClass Class => ScriptableObject.CreateInstance<CharacterClass>();
        public int Id => -1;
        public string Name => "NullBeast";
        public BeastTypeSO Type => ScriptableObject.CreateInstance<BeastTypeSO>();
    }
}