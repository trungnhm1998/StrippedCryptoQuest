using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Attributes;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Runtime.Battle.Builder
{
    public class CharacterBuilder
    {
        private const string ATTRIBUTE_SETS = "Assets/ScriptableObjects/Character/Attributes/AttributeSets.asset";
        private Elemental _elemental = An.Element.Fire;
        private GameObject _characterGameObject;
        public GameObject CharacterGameObject => _characterGameObject;

        private AttributeWithValue[] _stats =
        {
            new(AttributeSets.MaxHealth, 100f),
            new(AttributeSets.Health, 100f),
            new(AttributeSets.Strength, 50f),
        };

        public CharacterBuilder WithElement(Elemental elemental)
        {
            _elemental = elemental;
            return this;
        }

        public CharacterBuilder WithStats(AttributeWithValue[] stats)
        {
            _stats = stats;
            return this;
        }

        public ICharacter Build()
        {
            var attributeSets = AssetDatabase.LoadAssetAtPath<AttributeSets>(ATTRIBUTE_SETS);
            _characterGameObject = new GameObject();
            _characterGameObject.AddComponent<AttributeSystemBehaviour>();
            _characterGameObject.AddComponent<EffectSystemBehaviour>();
            _characterGameObject.AddComponent<AbilitySystemBehaviour>();
            _characterGameObject.AddComponent<CryptoQuest.Battle.Components.Character>();
            var statsProvider = _characterGameObject.AddComponent<InlineStatsProvider>();
            statsProvider.ProvideStats(_stats);
            _characterGameObject.AddComponent<SimpleStatsInitializer>();
            _characterGameObject.AddComponent<Element>();
            var character = _characterGameObject.GetComponent<ICharacter>();

            character.Init(_elemental);
            return character;
        }
    }
}