using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Character;
using UnityEngine.Localization;

namespace CryptoQuest.Beast.ScriptableObjects
{
    public class NullBeast : IBeast
    {
        private static NullBeast _instance;
        public static NullBeast Instance => _instance ??= new NullBeast();
        public PassiveAbility Passive { get; }
        public LocalizedString LocalizedName { get; }
        public Elemental Elemental { get; }
        public CharacterClass Class { get; }
        public int Id { get; } = -1;
        public string Name { get; } = "NullBeast";
        public BeastTypeSO Type { get; }
    }
}