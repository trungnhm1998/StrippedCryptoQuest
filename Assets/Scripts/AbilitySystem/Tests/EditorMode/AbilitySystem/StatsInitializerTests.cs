using NUnit.Framework;
using UnityEngine;

namespace Indigames.AbilitySystem.Tests.AbilitySystem
{
    public class StatsInitializerTests
    {
        private GameObject _go;
        private AttributeSystemBehaviour _attributeSystem;
        private StatsInitializer _statInitializer;
        private InitializeAttributeDatabase _statDatabase;

        [SetUp]
        public void Setup()
        {
            _go = new GameObject();
            _attributeSystem = _go.AddComponent<AttributeSystemBehaviour>();
            _statDatabase = ScriptableObject.CreateInstance<InitializeAttributeDatabase>();
            _statInitializer = _go.AddComponent<StatsInitializer>();
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
            _statInitializer.InitStats(_statDatabase);
            Assert.IsTrue(_attributeSystem.HasAttribute(attribute));
            _attributeSystem.GetAttributeValue(attribute, out var value);
            Assert.AreEqual(inputValue, value.CurrentValue);
        }
    }
}
