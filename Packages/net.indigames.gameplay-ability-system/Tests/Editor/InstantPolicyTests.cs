using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using NUnit.Framework;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystemTests
{
    [TestFixture]
    public class InstantPolicyTests
    {
        private GameObject _gameObject;
        private AttributeScriptableObject _healthAttribute;
        private AttributeSystemBehaviour _attributeSystem;
        private EffectSystemBehaviour _effectSystem;
        private AbilitySystemBehaviour _abilitySystem;

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            _healthAttribute = ScriptableObject.CreateInstance<AttributeScriptableObject>();
        }

        [SetUp]
        public void Setup()
        {
            _gameObject = new GameObject();
            _attributeSystem = _gameObject.AddComponent<AttributeSystemBehaviour>();
            _effectSystem = _gameObject.AddComponent<EffectSystemBehaviour>();
            _abilitySystem = _gameObject.AddComponent<AbilitySystemBehaviour>();

            _abilitySystem.AttributeSystem = _attributeSystem;
            _abilitySystem.EffectSystem = _effectSystem;

            _effectSystem.AttributeSystem = _attributeSystem;
            _effectSystem.Owner = _abilitySystem;

            _attributeSystem.SetAttributeBaseValue(_healthAttribute, 100f);

            _attributeSystem.TryGetAttributeValue(_healthAttribute, out var health);
            Assert.AreEqual(health.CurrentValue, 100f);
        }

        [Test]
        public void ApplyEffectToSelf_InstantPolicy_ShouldModify()
        {
            var instantDef = ScriptableObject.CreateInstance<GameplayEffectDefinition>();
            instantDef.Policy = new InstantPolicy();
            instantDef.EffectDetails.Modifiers = new[]
            {
                new EffectAttributeModifier()
                {
                    Attribute = _healthAttribute,
                    OperationType = EAttributeModifierOperationType.Add,
                    Value = 10f
                }
            };

            var instantSpec = _effectSystem.GetEffect(instantDef);
            _effectSystem.ApplyEffectToSelf(instantSpec);
            
            _attributeSystem.TryGetAttributeValue(_healthAttribute, out var health);
            Assert.AreEqual(110f, health.CurrentValue);
        }
    }
}