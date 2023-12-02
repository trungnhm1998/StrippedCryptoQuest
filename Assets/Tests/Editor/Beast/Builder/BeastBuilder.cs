using CryptoQuest.AbilitySystem.Abilities;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Beast;
using CryptoQuest.Character;
using NSubstitute;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tests.Editor.Beast.Builder
{
    public class BeastBuilder
    {
        private Elemental _element = ScriptableObject.CreateInstance<Elemental>();

        public BeastBuilder WithElement(Elemental element)
        {
            _element = element;
            return this;
        }

        private CharacterClass _characterClass;
        private BeastTypeSO _type;
        private PassiveAbility _passiveAbility;
        private int _level;
        private LocalizedString _name = new();

        public BeastBuilder WithClass(CharacterClass characterClass)
        {
            _characterClass = characterClass;
            return this;
        }

        public BeastBuilder WithType(BeastTypeSO type)
        {
            _type = type;
            return this;
        }

        public BeastBuilder WithPassive(PassiveAbility passiveAbility)
        {
            _passiveAbility = passiveAbility;
            return this;
        }

        public BeastBuilder WithLevel(int level)
        {
            _level = level;
            return this;
        }
        
        public BeastBuilder WithName(LocalizedString name)
        {
            _name = name;
            return this;
        }

        public IBeast Build()
        {
            var beast = Substitute.For<IBeast>();
            beast.Elemental.Returns(_element);
            beast.Class.Returns(_characterClass);
            beast.Type.Returns(_type);
            beast.Passive.Returns(_passiveAbility);
            // beast.Level.Returns(_level);
            beast.LocalizedName.Returns(_name);
            return beast;
        }
    }
}