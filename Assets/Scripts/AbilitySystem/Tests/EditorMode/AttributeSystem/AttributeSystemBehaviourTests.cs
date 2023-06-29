using NUnit.Framework;
using UnityEngine;

namespace Indigames.AbilitySystem.Tests.AttributeSystem
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
            Assert.IsFalse(_attributeSystem.HasAttribute(_attributeInSystem),
                $"{_attributeInSystem.name} is not added into Attribute System");
            Assert.IsFalse(_attributeSystem.HasAttribute(_attributeOutSystem),
                $"{_attributeOutSystem.name} is also not added into Attribute System");
        }

        [Test]
        public void AddAttribute_ShouldHasAddedAttributeOnly()
        {
            SetupAddAttribute();

            Assert.IsTrue(_attributeSystem.HasAttribute(_attributeInSystem),
                $"{_attributeInSystem} just added into Attribute System");
            Assert.IsFalse(_attributeSystem.HasAttribute(_attributeOutSystem),
                $"{_attributeOutSystem.name} still not added into Attribute System");
        }

        private void SetupAddAttribute()
        {
            _attributeSystem.AddAttributes(new AttributeScriptableObject[] {_attributeInSystem});
        }


        [Test]
        public void GetAttributeValue_ShouldTrueWithAddedAttribute()
        {
            SetupAddAttribute();

            Assert.IsTrue(_attributeSystem.GetAttributeValue(_attributeInSystem, out var valueInSystem), 
                $"Shoud be true because {_attributeInSystem.name} is in system");
            Assert.IsFalse(_attributeSystem.GetAttributeValue(_attributeOutSystem, out var valueOutSystem), 
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

            _attributeSystem.GetAttributeValue(_attributeInSystem, out var valueInSystem);
            _attributeSystem.GetAttributeValue(_attributeOutSystem, out var valueOutSystem);
            
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
            _attributeSystem.GetAttributeValue(_attributeInSystem, out var valueInSystem);
            
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
            _attributeSystem.GetAttributeValue(_attributeInSystem, out var value);
            Assert.AreEqual(expectedCurrentValue, value.CurrentValue);
        }

        [Test]
        [TestCase(10, 10)]
        [TestCase(100, 100)]
        public void UpdateAllAttributeCurrentValues_CurrentValueShouldEqualInput(float inputBaseValue, float expectedCurrentValue)
        {
            SetupAddAttribute();
            SetupSetBaseValueAttribute(inputBaseValue);

            _attributeSystem.UpdateAllAttributeCurrentValues();
            _attributeSystem.GetAttributeValue(_attributeInSystem, out var value);
            Assert.AreEqual(expectedCurrentValue, value.CurrentValue);
        }

        [Test]
        [TestCase(10, 1, 0, 0, 10, 11, EEffectStackingType.External)]
        [TestCase(10, 2, 0, 0, 12, 12, EEffectStackingType.Core)]
        [TestCase(10, -1, 0, 0, 10, 9, EEffectStackingType.External)]
        [TestCase(10, -1, 0, 0, 9, 9, EEffectStackingType.Core)]
        [TestCase(10, 0, 1, 0, 10, 20, EEffectStackingType.External)]
        [TestCase(10, 0, 1, 0, 20, 20, EEffectStackingType.Core)]
        [TestCase(10, 0, 0.5f, 0, 10, 15, EEffectStackingType.External)]
        [TestCase(10, 0, 0.5f, 0, 15, 15, EEffectStackingType.Core)]
        [TestCase(10, 1, 1, 0, 10, 22, EEffectStackingType.External)]
        [TestCase(10, 1, 1, 0, 22, 22, EEffectStackingType.Core)]
        [TestCase(10, 1, 1, 2, 10, 2, EEffectStackingType.External)]
        [TestCase(10, 1, 1, 2, 2, 2, EEffectStackingType.Core)]
        public void AddModifier_ShouldReturnExpectedValue(float inputBaseValue,
            float inputModifierAdditiveValue, float inputModifierMultiplyValue, float inputModifierOverrideValue,
            float expectedCoretValue, float expectedCurrentValue,
            EEffectStackingType stackMode)
        {
            SetupSetBaseValueAttribute(inputBaseValue);
            _attributeSystem.UpdateAttributeCurrentValue(_attributeInSystem);
            var modifier = new Modifier()
            {
                Additive = inputModifierAdditiveValue,
                Multiplicative = inputModifierMultiplyValue,
                Overriding = inputModifierOverrideValue
            };
            _attributeSystem.AddModifierToAttribute(modifier, _attributeInSystem, out _, stackMode);
            _attributeSystem.UpdateAttributeCurrentValue(_attributeInSystem);
            _attributeSystem.GetAttributeValue(_attributeInSystem, out var value);
            Assert.AreEqual(expectedCoretValue, AttributeSystemHelper.CaculateCoreAttributeValue(value));
            Assert.AreEqual(expectedCurrentValue, value.CurrentValue);
        }

        [Test]
        [TestCase(10, 1, 1, EEffectStackingType.External, 10)]
        [TestCase(10, -1, 1, EEffectStackingType.Core, 10)]
        public void ResetAttributeModifiers_ShouldReturnExpectedValue(float inputBaseValue, 
            float inputModifierAdditiveValue, float inputModifierMultiplyValue,
            EEffectStackingType stackMode, float expectedCurrentValue)
        {
            SetupSetBaseValueAttribute(inputBaseValue);
            var modifier = new Modifier()
            {
                Additive = inputModifierAdditiveValue,
                Multiplicative = inputModifierMultiplyValue
            };
            _attributeSystem.AddModifierToAttribute(modifier, _attributeInSystem, out _, stackMode);

            _attributeSystem.ResetAttributeModifiers();
            _attributeSystem.UpdateAllAttributeCurrentValues();
            _attributeSystem.GetAttributeValue(_attributeInSystem, out var value);
            Assert.AreEqual(expectedCurrentValue, value.CurrentValue);
        }
    }
}
