using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Implementation.BasicEffect;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Implementation.EffectAbility.TargetTypes;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.Tests.Runtime;
using NUnit.Framework;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.Tests.Implementation
{
    public class EffectAbilityTests
    {
        private const float DEFAULT_DURATION = 1;
        private const float DEFAULT_ADD_VALUE = 1;
        private const float DEFAULT_MUL_VALUE = 1;

        private GameObject _abilityOwner;
        private AbilitySystemBehaviour _abilitySystem;
        private EffectSystemBehaviour _effectSystem;
        private AttributeSystemBehaviour _attributeSystem;

        private DurationalEffectScriptableObject _durationEffectSO;
        private DurationalEffectScriptableObject _durationEffectSO2;
        private AttributeScriptableObject _attribute;
        private TagScriptableObject _otherTag;
        private TagScriptableObject _onActiveTag;
        private EffectAbilitySO _effectAbilitySO;
        private SelfTargetSO _selfTargetSO;

        private class MockParameters : AbilityParameters {}

        [SetUp]
        public void Setup()
        {
            _abilityOwner = new GameObject();
            _abilitySystem = _abilityOwner.AddComponent<AbilitySystemBehaviour>();
            _effectSystem = _abilitySystem.EffectSystem;
            _attributeSystem = _abilitySystem.AttributeSystem;
            _attribute = ScriptableObject.CreateInstance<AttributeScriptableObject>();
            _selfTargetSO = ScriptableObject.CreateInstance<SelfTargetSO>();

            _durationEffectSO = ScriptableObject.CreateInstance<DurationalEffectScriptableObject>();
            EffectTestHelper.SetupEffectSO(_attribute, _durationEffectSO, EAttributeModifierType.Add, DEFAULT_ADD_VALUE);
            _durationEffectSO.Duration = DEFAULT_DURATION;
            _durationEffectSO2 = ScriptableObject.CreateInstance<DurationalEffectScriptableObject>();
            EffectTestHelper.SetupEffectSO(_attribute, _durationEffectSO2, EAttributeModifierType.Multiply, DEFAULT_MUL_VALUE);
            _durationEffectSO2.Duration = DEFAULT_DURATION;

            _otherTag = ScriptableObject.CreateInstance<TagScriptableObject>();
            _onActiveTag = ScriptableObject.CreateInstance<TagScriptableObject>();
            _onActiveTag.name = "OnActive";

            _effectAbilitySO = ScriptableObject.CreateInstance<EffectAbilitySO>();
            AddEffectContainer(_effectAbilitySO, _durationEffectSO, _onActiveTag);
            AddEffectContainer(_effectAbilitySO, _durationEffectSO2, _otherTag);
        }

        private void AddEffectContainer(EffectAbilitySO abilitySO, EffectScriptableObject effectSO, TagScriptableObject tag)
        {
            var _effectMap = new EffectAbilitySO.EffectTagMap()
            {
                Tag = tag,
            };
            var _effectContainer = new AbilityEffectContainer();
            _effectContainer.Effects = new EffectScriptableObject[] {effectSO};
            _effectContainer.TargetType = _selfTargetSO;
            _effectMap.TargetContainer.Add(_effectContainer);

            abilitySO.EffectContainerMap.Add(_effectMap);
        }

        [Test]
        [TestCase(10, 11)]
        public void TryActiveAbility_ShouldOnlyApply_OnActiveTagEffect(float baseValue, float expectedValue)
        {
            ActivateAbility(baseValue, false);
            AssertCurrentAttribute(expectedValue);
        }

        [Test]
        [TestCase(10, 22)]
        public void TryActiveAbility_ApplyEffectContainerByTag_ShouldAlsoActivateOtherTag(float baseValue, float expectedValue)
        {
            ActivateAbility(baseValue, true);
            AssertCurrentAttribute(expectedValue);
        }

        [Test]
        [TestCase(10, 11)]
        public void RemoveEffectWithTag_ValueShouldBeCorrect(float baseValue, float expectedValue)
        {
            var ability = ActivateAbility(baseValue, true) as EffectAbility;

            ability.RemoveEffectWithTag(_otherTag);
            AssertCurrentAttribute(expectedValue);
        }

        [Test]
        [TestCase(10)]
        public void RemoveAllAbilities_ShouldAlsoRemoveEffect(float baseValue)
        {
            var ability = ActivateAbility(baseValue, true) as EffectAbility;

            _abilitySystem.RemoveAllAbilities();
            AssertCurrentAttribute(baseValue);
        }

        private AbstractAbility ActivateAbility(float baseValue, bool isActiveOtherTag)
        {
            _attributeSystem.AddAttributes(_attribute);
            _attributeSystem.SetAttributeBaseValue(_attribute, baseValue);

            var ability = _abilitySystem.GiveAbility(_effectAbilitySO) as EffectAbility;
            _abilitySystem.TryActiveAbility(ability);
            if (isActiveOtherTag)
            {
                ability.ApplyEffectContainerByTag(_otherTag, new EffectAbility.EffectAbilityContext());
            }
            return ability;
        }

        private void AssertCurrentAttribute(float expectedValue)
        {
            _effectSystem.ForceUpdateAttributeSystemModifiers();
            _attributeSystem.TryGetAttributeValue(_attribute, out var value);
            Assert.AreEqual(expectedValue, value.CurrentValue);
        }

    }
}
