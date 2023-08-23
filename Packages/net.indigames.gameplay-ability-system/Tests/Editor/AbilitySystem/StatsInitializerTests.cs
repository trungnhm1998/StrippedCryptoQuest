using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using NUnit.Framework;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.Tests.Editor.AbilitySystem
{
    public class StatsInitializerTests
    {
        private GameObject _go;
        private AttributeSystemBehaviour _attributeSystem;
        private ScriptableObjectStatsInitializer _scriptableObjectStatInitializer;
        private InitializeAttributeDatabase _statDatabase;

        [SetUp]
        public void Setup()
        {
            _go = new GameObject();
            _attributeSystem = _go.AddComponent<AttributeSystemBehaviour>();
            _statDatabase = ScriptableObject.CreateInstance<InitializeAttributeDatabase>();
            _scriptableObjectStatInitializer = _go.AddComponent<ScriptableObjectStatsInitializer>();
        }

        [Test]
        [TestCase(1)]
        public void InitStats_AttributeValues_ShouldBeEqualWithDatabase(float inputValue)
        {
            var attribute = ScriptableObject.CreateInstance<AttributeScriptableObject>();
            _statDatabase.AttributesToInitialize = new AttributeInitValue[]
            {
                new AttributeInitValue() 
                {
                    Attribute = attribute,
                    Value = inputValue
                }
            };
            _scriptableObjectStatInitializer.InitStats(_statDatabase);
            Assert.IsTrue(_attributeSystem.HasAttribute(attribute, out _));
            _attributeSystem.TryGetAttributeValue(attribute, out var value);
            Assert.AreEqual(inputValue, value.CurrentValue);
        }
    }
}
