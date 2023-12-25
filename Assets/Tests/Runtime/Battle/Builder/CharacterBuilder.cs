using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Runtime.Battle.Builder
{
    public class CharacterBuilder<T> where T : CryptoQuest.Battle.Components.Character
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

        public CharacterBuilder<T> WithElement(Elemental elemental)
        {
            _elemental = elemental;
            return this;
        }

        public CharacterBuilder<T> WithStats(AttributeWithValue[] stats)
        {
            _stats = stats;
            return this;
        }

        public T Build()
        {
            var attributeSets = AssetDatabase.LoadAssetAtPath<AttributeSets>(ATTRIBUTE_SETS);
            _characterGameObject = new GameObject();
            _characterGameObject.AddComponent<AttributeSystemBehaviour>();
            _characterGameObject.AddComponent<EffectSystemBehaviour>();
            _characterGameObject.AddComponent<AbilitySystemBehaviour>();
            _characterGameObject.AddComponent<T>();
            var statsProvider = _characterGameObject.AddComponent<InlineStatsProvider>();
            statsProvider.ProvideStats(_stats);
            _characterGameObject.AddComponent<SimpleStatsInitializer>();
            var elementComp = _characterGameObject.AddComponent<Element>();
            var character = _characterGameObject.GetComponent<T>();

            elementComp.SetElement(_elemental);
            character.Init();
            return character;
        }
    }
}