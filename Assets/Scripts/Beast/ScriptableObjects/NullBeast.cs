using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Character;
using CryptoQuest.Gameplay;
using UnityEngine.Localization;

namespace CryptoQuest.Beast.ScriptableObjects
{
    public class NullBeast : IBeast
    {
        private static NullBeast _instance;
        public static NullBeast Instance => _instance ??= new NullBeast();
        public PassiveAbility Passive => null;
        public LocalizedString LocalizedName => new();
        public Elemental Elemental => null;
        public CharacterClass Class => null;
        public string BeastId => string.Empty;
        public int Id => -1;
        public string Name => "NullBeast";
        public BeastTypeSO Type => null;
        public StatsDef Stats => null;
        public int Level => 1;
        public int MaxLevel => 99;
        public int Stars => 1;
        public bool IsValid() => false;
    }
}