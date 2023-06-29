using NUnit.Framework;
using UnityEngine;
using System.Collections;

namespace Indigames.AbilitySystem.Tests.EffectSystem
{
    public class EffectSystemBehaviourTests
    {
        private GameObject _abilityOwner;
        private AbilitySystemBehaviour _abilitySystem;
        private EffectSystemBehaviour _effectSystem;
        private EffectScriptableObject _instantEffectSO;
        private EffectScriptableObject _durationEffectSO;
        private AttributeScriptableObject _attribute;

        private class MockParameters : AbilityParameters {}

        [SetUp]
        public void Setup()
        {
            _abilityOwner = new GameObject();
            _abilitySystem = _abilityOwner.AddComponent<AbilitySystemBehaviour>();
            _effectSystem = _abilitySystem.EffectSystem;
            _effectSystem.InitSystem(_abilitySystem);
            _instantEffectSO = ScriptableObject.CreateInstance<InstantEffectScriptableObject>();
            _durationEffectSO = ScriptableObject.CreateInstance<DurationalEffectScriptableObject>();
            _attribute = ScriptableObject.CreateInstance<AttributeScriptableObject>();
        }

        [Test]
        public void GiveEffect_ReturnEffectSpec_CorrectOwner()
        {
            var effect = _effectSystem.GetEffect(_instantEffectSO, this, new MockParameters());
            Assert.IsNotNull(effect);
            Assert.AreEqual(effect.Owner, _abilitySystem);
        }

        [Test]
        [TestCase(EAttributeModifierType.Add, 10, 1, 11)]
        [TestCase(EAttributeModifierType.Multiply, 10, 2, 20)]
        [TestCase(EAttributeModifierType.Override, 10, 2, 2)]
        public void ApplyEffectToSelf_InstantEffect_ValueShouldCorrect(EAttributeModifierType modifierType, float baseValue, float effectValue, float expectedvalue)
        {
            _instantEffectSO.EffectDetails = new EffectDetails();
            _instantEffectSO.EffectDetails.Modifiers = new EffectAttributeModifier[] {
                new EffectAttributeModifier()
                {
                    AttributeSO = _attribute,
                    ModifierType = modifierType,
                    ModifierComputationMethod = null,
                    Value = effectValue
                }
            };
            _instantEffectSO.EffectDetails.StackingType = EEffectStackingType.External;
            
            _abilitySystem.AttributeSystem.SetAttributeBaseValue(_attribute, baseValue);
            var effect = _effectSystem.GetEffect(_instantEffectSO, this, new MockParameters());
            var applied = _effectSystem.ApplyEffectToSelf(effect);
            Assert.AreNotEqual(applied, NullEffect.Instance);
            _effectSystem.ForceUpdateAttributeSystemModifiers();
            _abilitySystem.AttributeSystem.GetAttributeValue(_attribute, out var value);
            Assert.AreEqual(expectedvalue, value.CurrentValue);
        }
    }
}
