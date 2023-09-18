using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Attributes;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Runtime.Battle.Builder
{
    public class CharacterBuilder
    {
        private const string ATTRIBUTE_SETS = "Assets/ScriptableObjects/Character/Attributes/AttributeSets.asset";
        private const string CHARACTER_PREFAB_ASSET = "Assets/Prefabs/Battle/Character.prefab";
        private Elemental _elemental = An.Element.Fire;
        private GameObject _characterGameObject;
        public GameObject CharacterGameObject => _characterGameObject;

        private AttributeWithValue[] _stats = new AttributeWithValue[]
        {
            new(AttributeSets.MaxHealth, 100f),
            new(AttributeSets.Health, 100f),
            new(AttributeSets.Strength, 50f),
        };


        public CharacterBuilder WithPrefab(GameObject prefab)
        {
            _characterGameObject = prefab;
            return this;
        }

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
            if (_characterGameObject == null)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(CHARACTER_PREFAB_ASSET);
                _characterGameObject = Object.Instantiate(prefab);
            }

            var character = _characterGameObject.GetComponent<ICharacter>();
            _characterGameObject.GetComponent<Element>().SetElement(_elemental);
            var statsInitializer = _characterGameObject.GetComponent<IStatsInitializer>();
            statsInitializer.SetStats(_stats);
            character.Init();
            return character;
        }
    }
}