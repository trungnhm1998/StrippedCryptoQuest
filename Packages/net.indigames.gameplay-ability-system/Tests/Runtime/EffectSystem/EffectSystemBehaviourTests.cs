using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;
using IndiGames.GameplayAbilitySystem.Tests.Runtime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace IndiGames.GameplayAbilitySystem.Tests.EffectSystem
{
    public class EffectSystemBehaviourTests
    {
        private GameObject _abilityOwner;
        private AbilitySystemBehaviour _abilitySystem;
        private EffectSystemBehaviour _effectSystem;
        private AttributeSystemBehaviour _attributeSystem;
        private InstantEffectScriptableObject _instantEffectSO;
        private DurationalEffectScriptableObject _durationEffectSO;
        private AttributeScriptableObject _attribute;
        private TagScriptableObject _requiredTag;
        private TagScriptableObject _ignoredTag;

        [SetUp]
        public void Setup()
        {
            _abilityOwner = new GameObject();
            _abilitySystem = _abilityOwner.AddComponent<AbilitySystemBehaviour>();
            _effectSystem = _abilitySystem.EffectSystem;
            _attributeSystem = _abilitySystem.AttributeSystem;
            _instantEffectSO = ScriptableObject.CreateInstance<InstantEffectScriptableObject>();
            _durationEffectSO = ScriptableObject.CreateInstance<DurationalEffectScriptableObject>();
            _attribute = ScriptableObject.CreateInstance<AttributeScriptableObject>();
            _requiredTag = ScriptableObject.CreateInstance<TagScriptableObject>();
            _ignoredTag = ScriptableObject.CreateInstance<TagScriptableObject>();
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
        [TestCase(EAttributeModifierType.Multiply, 10, 1, 20)]
        [TestCase(EAttributeModifierType.Override, 10, 2, 2)]
        public void ApplyEffectToSelf_InstantEffect_ValueShouldCorrect(EAttributeModifierType modifierType, float baseValue, float effectValue, float expectedValue)
        {
            SetupAndApplyEffect(_instantEffectSO, modifierType, effectValue, 10);
            _attributeSystem.GetAttributeValue(_attribute, out var value);
            Assert.AreEqual(expectedValue, value.BaseValue);
        }

        [Test]
        public void ApplyEffectToSelf_ShouldOnlyApply_WhenSystemHasRequiredTag()
        {
            _instantEffectSO.ApplicationTagRequirements.RequireTags = new TagScriptableObject[] {_requiredTag};
            var effect = SetupAndApplyEffect(_instantEffectSO);
            Assert.AreEqual(effect, NullEffect.Instance);

            _abilitySystem.TagSystem.GrantedTags.Add(_requiredTag);
            effect = _effectSystem.GetEffect(_instantEffectSO, this, new MockParameters());
            var appliedEffect = _effectSystem.ApplyEffectToSelf(effect);
            Assert.AreNotEqual(effect, NullEffect.Instance);
        }

        [Test]
        public void ApplyEffectToSelf_ShouldOnlyApply_WhenSystemHasNoneIgnoredTag()
        {
            _instantEffectSO.ApplicationTagRequirements.IgnoreTags = new TagScriptableObject[] {_ignoredTag};
            _abilitySystem.TagSystem.GrantedTags.Add(_ignoredTag);
            var effect = SetupAndApplyEffect(_instantEffectSO);
            Assert.AreEqual(effect, NullEffect.Instance);

            _abilitySystem.TagSystem.GrantedTags.Remove(_ignoredTag);
            effect = _effectSystem.GetEffect(_instantEffectSO, this, new MockParameters());
            var appliedEffect = _effectSystem.ApplyEffectToSelf(effect);
            Assert.AreNotEqual(effect, NullEffect.Instance);
        }

        
        [Test]
        public void ApplyEffectToSelf_ShouldNotApply_IfModifierAttributeInvalid()
        {
            _instantEffectSO.EffectDetails = new EffectDetails();
            _instantEffectSO.EffectDetails.Modifiers = new EffectAttributeModifier[] {
                new EffectAttributeModifier()
                {
                    AttributeSO = null,
                    ModifierType = EAttributeModifierType.Add,
                    ModifierComputationMethod = null,
                    Value = 1
                }
            };
            var effect = _effectSystem.GetEffect(_instantEffectSO, this, new MockParameters());
            var appliedEffect = _effectSystem.ApplyEffectToSelf(effect);
            Assert.AreEqual(appliedEffect, NullEffect.Instance);
        }

        [Test]
        [TestCase(0, 1f, ExpectedResult = true)] // dont need seed
        [TestCase(0, 0f, ExpectedResult = false)] // dont need seed
        [TestCase(152, 0.5f, ExpectedResult = false)] // Random seed 152 = 0.6869418
        [TestCase(42, 0.5f, ExpectedResult = true)] // Random seed 42 = 0.000857711
        public bool ApplyEffectToSelf_ShouldOnlyApply_IfChanceToApplyMet(int randomInitState, float chanceToApply)
        {
            Random.InitState(randomInitState);
            _instantEffectSO.ChanceToApply = chanceToApply;
            var effect = SetupAndApplyEffect(_instantEffectSO);
            return effect != NullEffect.Instance;
        }

        [Test]
        public void ApplyEffectToSelf_ShouldOnlyApply_IfCustomRequirementMet()
        {
            _instantEffectSO.CustomApplicationRequirements.Add(new FalseCustomRequirement());
            var effect = SetupAndApplyEffect(_instantEffectSO);
            Assert.IsTrue(effect == NullEffect.Instance);

            _instantEffectSO.CustomApplicationRequirements.Add(new TrueCustomRequirement());
            effect = SetupAndApplyEffect(_instantEffectSO);
            Assert.IsTrue(effect == NullEffect.Instance);

            _instantEffectSO.CustomApplicationRequirements.RemoveAll(x => x is FalseCustomRequirement);
            effect = SetupAndApplyEffect(_instantEffectSO);
            Assert.IsFalse(effect == NullEffect.Instance);
        }

        [UnityTest]
        [TestCase(EAttributeModifierType.Add, 10, 1, 11, 1, ExpectedResult = null)]
        [TestCase(EAttributeModifierType.Multiply, 10, 1, 20, 1, ExpectedResult = null)]
        [TestCase(EAttributeModifierType.Override, 10, 2, 2, 1, ExpectedResult = null)]
        public IEnumerator ApplyEffectToSelf_DurationalEffect_ValueShouldCorrect_ReturnToBaseAfterDuration(EAttributeModifierType modifierType, 
            float baseValue, float effectValue, float expectedValue,
            float duration)
        {
            var value = new AttributeValue();
            _durationEffectSO.Duration = duration;
            SetupAndApplyEffect(_durationEffectSO, modifierType, effectValue, baseValue);
            _effectSystem.ForceUpdateAttributeSystemModifiers();

            _attributeSystem.GetAttributeValue(_attribute, out value);
            Assert.AreEqual(expectedValue, value.CurrentValue);
            yield return new WaitForSeconds(duration);
            _effectSystem.ForceUpdateAttributeSystemModifiers();


            _attributeSystem.GetAttributeValue(_attribute, out value);
            Assert.AreEqual(baseValue, value.CurrentValue);
        }

        [UnityTest]
        [TestCase(EAttributeModifierType.Add, 10, 1, 10, ExpectedResult = null)]
        [TestCase(EAttributeModifierType.Multiply, 10, 1, 10, ExpectedResult = null)]
        [TestCase(EAttributeModifierType.Override, 10, 2, 10, ExpectedResult = null)]
        public IEnumerator RemoveEffect_DurationalEffect_ValueReturnToBaseAfterRemove(EAttributeModifierType modifierType, 
            float baseValue, float effectValue, 
            float duration)
        {
            var value = new AttributeValue();
            _durationEffectSO.Duration = duration;
            var effect = SetupAndApplyEffect(_durationEffectSO, modifierType, effectValue, baseValue);

            yield return new WaitForFixedUpdate();
            Assert.AreNotEqual(baseValue, value.CurrentValue);

            _effectSystem.RemoveEffect(effect);

            _attributeSystem.GetAttributeValue(_attribute, out value);
            Assert.AreEqual(baseValue, value.CurrentValue);
        }
        
        private AbstractEffect SetupAndApplyEffect(EffectScriptableObject effectSO,
            EAttributeModifierType modifierType = EAttributeModifierType.Add,
            float value = 1, float baseValue = 10)
        {
            return EffectTestHelper.SetupAndApplyEffect(_abilitySystem, _attribute, effectSO, modifierType, value, baseValue);
        }
    }
}
