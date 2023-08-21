using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.Helper;
using NUnit.Framework;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.Tests.Editor.AttributeSystem
{
    public class AttributeSystemBehaviourTests
    {
        private const float DEFAULT_ATTRIBUTE_VALUE = 0;
        private GameObject _gameObject;
        private AttributeSystemBehaviour _attributeSystem;
        private AttributeScriptableObject _attributeInSystem;
        private AttributeScriptableObject _attributeOutSystem;

        [SetUp]
        public void Setup()
        {
            _gameObject = new GameObject();
            _attributeSystem = _gameObject.AddComponent<AttributeSystemBehaviour>();
            _attributeInSystem = ScriptableObject.CreateInstance<AttributeScriptableObject>();
            _attributeOutSystem = ScriptableObject.CreateInstance<AttributeScriptableObject>();
        }

        [Test]
        public void HasAttribute_ShouldFalse()
        {
            Assert.IsFalse(_attributeSystem.HasAttribute(_attributeInSystem, out _),
                $"{_attributeInSystem.name} is not added into Attribute System");
            Assert.IsFalse(_attributeSystem.HasAttribute(_attributeOutSystem, out _),
                $"{_attributeOutSystem.name} is also not added into Attribute System");
        }

        [Test]
        public void AddAttribute_ShouldHasAddedAttributeOnly()
        {
            SetupAddAttribute();

            Assert.IsTrue(_attributeSystem.HasAttribute(_attributeInSystem, out _),
                $"{_attributeInSystem} just added into Attribute System");
            Assert.IsFalse(_attributeSystem.HasAttribute(_attributeOutSystem, out _),
                $"{_attributeOutSystem.name} still not added into Attribute System");
        }

        private void SetupAddAttribute()
        {
            _attributeSystem.AddAttributes(_attributeInSystem);
        }


        [Test]
        public void GetAttributeValue_ShouldTrueWithAddedAttribute()
        {
            SetupAddAttribute();

            Assert.IsTrue(_attributeSystem.TryGetAttributeValue(_attributeInSystem, out var valueInSystem), 
                $"Shoud be true because {_attributeInSystem.name} is in system");
            Assert.IsFalse(_attributeSystem.TryGetAttributeValue(_attributeOutSystem, out var valueOutSystem), 
                $"Shoud be false because {_attributeOutSystem.name} is in system");
        }

        private void SetupSetBaseValueAttribute(float value)
        {
            SetupAddAttribute();

            _attributeSystem.SetAttributeBaseValue(_attributeInSystem, value);
            _attributeSystem.SetAttributeBaseValue(_attributeOutSystem, value);
        }

        [Test]
        [TestCase(10, 10)]
        [TestCase(20, 20)]
        public void SetAttributeBaseValue_ShouldEqualInputValue(float inputValue, float expectedBaseValue)
        {
            SetupSetBaseValueAttribute(inputValue);

            _attributeSystem.TryGetAttributeValue(_attributeInSystem, out var valueInSystem);
            _attributeSystem.TryGetAttributeValue(_attributeOutSystem, out var valueOutSystem);
            
            Assert.AreEqual(expectedBaseValue, valueInSystem.BaseValue);
            Assert.AreEqual(DEFAULT_ATTRIBUTE_VALUE, valueOutSystem.BaseValue,
                $"Because {_attributeOutSystem.name} not in the system\n So the system return default value");
        }
        
        [Test]
        [TestCase(-1)]
        [TestCase(100)]
        [TestCase(20)]
        public void ResetAllAttributes_ValueShouldEqualDefault(float inputValue)
        {
            SetupSetBaseValueAttribute(inputValue);

            _attributeSystem.ResetAllAttributes();
            _attributeSystem.TryGetAttributeValue(_attributeInSystem, out var valueInSystem);
            
            Assert.AreEqual(DEFAULT_ATTRIBUTE_VALUE, valueInSystem.BaseValue);
        }

        [Test]
        [TestCase(10, 10)]
        [TestCase(100, 100)]
        public void UpdateAttributeCurrentValue_CurrentValueShouldEqualInput(float inputBaseValue, float expectedCurrentValue)
        {
            SetupAddAttribute();
            SetupSetBaseValueAttribute(inputBaseValue);

            _attributeSystem.UpdateAttributeCurrentValue(_attributeInSystem);
            _attributeSystem.TryGetAttributeValue(_attributeInSystem, out var value);
            Assert.AreEqual(expectedCurrentValue, value.CurrentValue);
        }

        [Test]
        [TestCase(10, 10)]
        [TestCase(100, 100)]
        public void UpdateAllAttributeCurrentValues_CurrentValueShouldEqualInput(float inputBaseValue, float expectedCurrentValue)
        {
            SetupAddAttribute();
            SetupSetBaseValueAttribute(inputBaseValue);

            _attributeSystem.UpdateAttributeValues();
            _attributeSystem.TryGetAttributeValue(_attributeInSystem, out var value);
            Assert.AreEqual(expectedCurrentValue, value.CurrentValue);
        }

        [Test]
        [TestCase(10, 1, 0, 0, 10, 11, EModifierType.External)]
        [TestCase(10, 2, 0, 0, 12, 12, EModifierType.Core)]
        [TestCase(10, -1, 0, 0, 10, 9, EModifierType.External)]
        [TestCase(10, -1, 0, 0, 9, 9, EModifierType.Core)]
        [TestCase(10, 0, 1, 0, 10, 20, EModifierType.External)]
        [TestCase(10, 0, 1, 0, 20, 20, EModifierType.Core)]
        [TestCase(10, 0, 0.5f, 0, 10, 15, EModifierType.External)]
        [TestCase(10, 0, 0.5f, 0, 15, 15, EModifierType.Core)]
        [TestCase(10, 1, 1, 0, 10, 22, EModifierType.External)]
        [TestCase(10, 1, 1, 0, 22, 22, EModifierType.Core)]
        [TestCase(10, 1, 1, 2, 10, 2, EModifierType.External)]
        [TestCase(10, 1, 1, 2, 2, 2, EModifierType.Core)]
        public void AddModifier_ShouldReturnExpectedValue(float inputBaseValue,
            float inputModifierAdditiveValue, float inputModifierMultiplyValue, float inputModifierOverrideValue,
            float expectedCoreValue, float expectedCurrentValue,
            EModifierType stackMode)
        {
            SetupSetBaseValueAttribute(inputBaseValue);
            _attributeSystem.UpdateAttributeCurrentValue(_attributeInSystem);
            var modifier = new Modifier()
            {
                Additive = inputModifierAdditiveValue,
                Multiplicative = inputModifierMultiplyValue,
                Overriding = inputModifierOverrideValue
            };
            _attributeSystem.TryAddModifierToAttribute(modifier, _attributeInSystem, stackMode);
            _attributeSystem.UpdateAttributeCurrentValue(_attributeInSystem);
            _attributeSystem.TryGetAttributeValue(_attributeInSystem, out var value);
            Assert.AreEqual(expectedCoreValue, AttributeSystemHelper.CaculateCoreAttributeValue(value));
            Assert.AreEqual(expectedCurrentValue, value.CurrentValue);
        }

        [Test]
        [TestCase(10, 1, 1, EModifierType.External, 10)]
        [TestCase(10, -1, 1, EModifierType.Core, 10)]
        public void ResetAttributeModifiers_ShouldReturnExpectedValue(float inputBaseValue, 
            float inputModifierAdditiveValue, float inputModifierMultiplyValue,
            EModifierType modifierType, float expectedCurrentValue)
        {
            SetupSetBaseValueAttribute(inputBaseValue);
            var modifier = new Modifier()
            {
                Additive = inputModifierAdditiveValue,
                Multiplicative = inputModifierMultiplyValue
            };
            _attributeSystem.TryAddModifierToAttribute(modifier, _attributeInSystem, modifierType);

            _attributeSystem.ResetAttributeModifiers();
            _attributeSystem.UpdateAttributeValues();
            _attributeSystem.TryGetAttributeValue(_attributeInSystem, out var value);
            Assert.AreEqual(expectedCurrentValue, value.CurrentValue);
        }
    }
}
