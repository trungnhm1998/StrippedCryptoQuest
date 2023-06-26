using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Indigames.AbilitySystem;

namespace Indigames.AbilitySystem.Tests.Attribute
{
    public class AttributeSystemTests
    {
        private const float DEFAULT_ATTRIBUTE_VALUE = 0;
        private GameObject _gameObject;
        private AttributeSystem _attributeSystem;
        private AttributeScriptableObject _attributeInSystem;
        private AttributeScriptableObject _attributeOutSystem;

        [SetUp]
        public void Setup()
        {
            _gameObject = new GameObject();
            _attributeSystem = _gameObject.AddComponent<AttributeSystem>();
            _attributeInSystem = ScriptableObject.CreateInstance<AttributeScriptableObject>();
            _attributeInSystem.name = "TestAttributeInSystem";
            _attributeOutSystem = ScriptableObject.CreateInstance<AttributeScriptableObject>();
            _attributeOutSystem.name = "TestAttributeOutSystem";
        }

        [Test]
        public void AttributeSystem_CheckHasAttribute()
        {
            Assert.IsFalse(_attributeSystem.HasAttribute(_attributeInSystem),
                $"{_attributeInSystem.name} is not added into Attribute System");
            Assert.IsFalse(_attributeSystem.HasAttribute(_attributeOutSystem),
                $"{_attributeOutSystem.name} is also not added into Attribute System");
        }

        [Test]
        public void AttributeSystem_CheckAddAttribute()
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
        public void AttributeSystem_GetAttributeValue()
        {
            AttributeSystem_CheckAddAttribute();

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
        public void AttributeSystem_SetBaseAttributeValueTo10()
        {
            SetupSetBaseValueAttribute(10);

            _attributeSystem.GetAttributeValue(_attributeInSystem, out var valueInSystem);
            _attributeSystem.GetAttributeValue(_attributeOutSystem, out var valueOutSystem);
            
            Assert.AreEqual(10, valueInSystem.BaseValue);
            Assert.AreEqual(DEFAULT_ATTRIBUTE_VALUE, valueOutSystem.BaseValue,
                $"Because {_attributeOutSystem.name} not in the system\n So the system return default value");
        }
        
        [Test]
        public void AttributeSystem_ResetAllAttributes()
        {
            SetupSetBaseValueAttribute(100);

            _attributeSystem.ResetAllAttributes();
            _attributeSystem.GetAttributeValue(_attributeInSystem, out var valueInSystem);
            
            Assert.AreEqual(DEFAULT_ATTRIBUTE_VALUE, valueInSystem.BaseValue);
        }

        
        [Test]
        public void AttributeSystem_UpdateAttributeCurrentValue()
        {
            SetupAddAttribute();
            SetupSetBaseValueAttribute(10);
            AttributeValue value = new AttributeValue();
            _attributeSystem.GetAttributeValue(_attributeInSystem, out value);
            Assert.AreEqual(10, value.BaseValue);

            Assert.AreEqual(0, value.CurrentValue);
            _attributeSystem.UpdateAttributeCurrentValue(_attributeInSystem);
            _attributeSystem.GetAttributeValue(_attributeInSystem, out value);
            Assert.AreEqual(10, value.CurrentValue);

            SetupSetBaseValueAttribute(0);

            Assert.AreEqual(10, value.CurrentValue);
            _attributeSystem.UpdateAllAttributeCurrentValues();
            _attributeSystem.GetAttributeValue(_attributeInSystem, out value);
            Assert.AreEqual(0, value.CurrentValue);
        }

        [Test]
        public void AttributeSystem_AddModifierAdd_1()
        {
            SetupSetBaseValueAttribute(10);
            _attributeSystem.UpdateAttributeCurrentValue(_attributeInSystem);
            AttributeValue value = new AttributeValue();
            _attributeSystem.GetAttributeValue(_attributeInSystem, out value);
            var modifier = new Modifier()
            {
                Additive = 1
            };
            //Modifier External so the CoreValue unchanged
            Assert.AreEqual(0, value.Modifier.Additive);
            _attributeSystem.AddModifierToAttribute(modifier, _attributeInSystem, out value);
            Assert.AreEqual(1, value.Modifier.Additive);
            Assert.AreEqual(10, value.CurrentValue);
            _attributeSystem.UpdateAttributeCurrentValue(_attributeInSystem);
            _attributeSystem.GetAttributeValue(_attributeInSystem, out value);
            Assert.AreEqual(10, AttributeSystemHelper.CaculateCoreAttributeValue(value));
            Assert.AreEqual(11, value.CurrentValue);

            //Modifier Core and the CoreValue changed
            Assert.AreEqual(0, value.CoreModifier.Additive);
            _attributeSystem.AddModifierToAttribute(modifier, _attributeInSystem, out value, EEffectStackingType.Core);
            Assert.AreEqual(1, value.CoreModifier.Additive);
            _attributeSystem.UpdateAllAttributeCurrentValues();
            _attributeSystem.GetAttributeValue(_attributeInSystem, out value);
            Assert.AreEqual(11, AttributeSystemHelper.CaculateCoreAttributeValue(value));
            Assert.AreEqual(12, value.CurrentValue);
        }

        
        [Test]
        public void AttributeSystem_AddModifierMultiply_100Percent()
        {
            SetupSetBaseValueAttribute(10);
            _attributeSystem.UpdateAttributeCurrentValue(_attributeInSystem);
            AttributeValue value = new AttributeValue();
            _attributeSystem.GetAttributeValue(_attributeInSystem, out value);
            var modifier = new Modifier()
            {
                Multiplicative = 1
            };

            //Modifier External so the CoreValue unchanged
            Assert.AreEqual(0, value.Modifier.Multiplicative);
            _attributeSystem.AddModifierToAttribute(modifier, _attributeInSystem, out value);
            Assert.AreEqual(1, value.Modifier.Multiplicative);
            Assert.AreEqual(10, value.CurrentValue);
            _attributeSystem.UpdateAttributeCurrentValue(_attributeInSystem);
            _attributeSystem.GetAttributeValue(_attributeInSystem, out value);
            Assert.AreEqual(10, AttributeSystemHelper.CaculateCoreAttributeValue(value));
            Assert.AreEqual(20, value.CurrentValue);

            //Modifier Core and the CoreValue changed
            Assert.AreEqual(0, value.CoreModifier.Multiplicative);
            _attributeSystem.AddModifierToAttribute(modifier, _attributeInSystem, out value, EEffectStackingType.Core);
            Assert.AreEqual(1, value.CoreModifier.Multiplicative);
            _attributeSystem.UpdateAllAttributeCurrentValues();
            _attributeSystem.GetAttributeValue(_attributeInSystem, out value);
            Assert.AreEqual(20, AttributeSystemHelper.CaculateCoreAttributeValue(value));
            Assert.AreEqual(40, value.CurrentValue);
        }

        [Test]
        public void AttributeSystem_ResetAllModifiers()
        {
            AttributeSystem_AddModifierMultiply_100Percent();
            AttributeValue value = new AttributeValue();
            _attributeSystem.GetAttributeValue(_attributeInSystem, out value);
            Assert.AreEqual(40, value.CurrentValue);

            _attributeSystem.ResetAttributeModifiers();
            _attributeSystem.UpdateAllAttributeCurrentValues();
            _attributeSystem.GetAttributeValue(_attributeInSystem, out value);
            Assert.AreEqual(10, value.CurrentValue);
        }
    }
}
