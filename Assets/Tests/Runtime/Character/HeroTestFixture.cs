using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace CryptoQuest.Tests.Runtime.Character
{
    public class HeroTestFixture
    {
        private EffectSystemBehaviour _effectSystemBehaviour;
        protected EffectSystemBehaviour EffectSystem => _effectSystemBehaviour;
        private AbilitySystemBehaviour _abilitySystem;
        protected AbilitySystemBehaviour AbilitySystem => _abilitySystem;
        private GameObject _hero;
        protected GameObject Hero => _hero;
        private AttributeSystemBehaviour _attributeSystemBehaviour;
        public AttributeSystemBehaviour AttributeSystem => _attributeSystemBehaviour;


        [SetUp]
        public virtual void Setup()
        {
            AssetDatabase.LoadAssetAtPath<AttributeSets>(
                "Assets/ScriptableObjects/Character/Attributes/AttributeSets.asset");

            _hero = new GameObject("Hero");
            _attributeSystemBehaviour = _hero.AddComponent<AttributeSystemBehaviour>();
            _effectSystemBehaviour = _hero.AddComponent<EffectSystemBehaviour>();
            _hero.AddComponent<TagSystemBehaviour>();
            _abilitySystem = _hero.AddComponent<AbilitySystemBehaviour>();
            _hero.AddComponent<HeroBehaviour>();
        }

        protected void SetAttribute(AttributeScriptableObject attribute, float value)
        {
            AttributeSystem.SetAttributeBaseValue(attribute, value);
        }
    }
}