using System.Collections;
using IndiGames.GameplayAbilitySystem.AbilitySystem;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using NUnit.Framework;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.Tests.Editor.AbilitySystem
{
    public class AbilitySystemBehaviourTests
    {
        private GameObject _abilityOwner;
        private AbilitySystemBehaviour _abilitySystem;
        private AbilityScriptableObject _abilitySO;
        private TagScriptableObject _requiredTag;
        private TagScriptableObject _ignoredTag;

        private class MockAbilitySO : AbilityScriptableObject<MockAbility>
        {
        }

        private class MockAbility : AbstractAbility
        {
            protected override IEnumerator InternalActiveAbility()
            {
                yield return null;
            }
        }

        [SetUp]
        public void Setup()
        {
            _abilityOwner = new GameObject();
            _abilitySystem = _abilityOwner.AddComponent<AbilitySystemBehaviour>();
            _abilitySO = ScriptableObject.CreateInstance<MockAbilitySO>();
            _requiredTag = ScriptableObject.CreateInstance<TagScriptableObject>();
            _ignoredTag = ScriptableObject.CreateInstance<TagScriptableObject>();
        }

        [Test]
        public void OnValidate_ShouldAssignComponentsCorrectly()
        {
            Assert.IsNotNull(_abilitySystem.AttributeSystem, $"{_abilityOwner} has no Attribute System.");
            Assert.IsNotNull(_abilitySystem.TagSystem, $"{_abilityOwner} has no Tag System.");
            Assert.IsNotNull(_abilitySystem.EffectSystem, $"{_abilityOwner} has no Effect System.");
        }

        [Test]
        public void GiveAbility_ShouldContainAbilitySpec()
        {
            var ability = _abilitySystem.GiveAbility(_abilitySO);
            Assert.IsTrue(_abilitySystem.GrantedAbilities.Abilities.Contains(ability));
            Assert.AreEqual(_abilitySystem, ability.Owner);
        }

        [Test]
        public void GiveAbility_Twice_ShouldNotContainDuplicateSpec()
        {
            _abilitySystem.GiveAbility(_abilitySO);
            _abilitySystem.GiveAbility(_abilitySO);
            Assert.AreEqual(1, _abilitySystem.GrantedAbilities.Abilities.Count);
        }
        
        [Test]
        public void TryActiveAbility_AbilityShouldActive()
        {
            _abilitySO.Tags.ActivationTags = new TagScriptableObject[] {_requiredTag};
            var ability = _abilitySystem.GiveAbility(_abilitySO);
            _abilitySystem.TryActiveAbility(ability);
            Assert.IsTrue(ability.IsActive);
            Assert.IsTrue(_abilitySystem.TagSystem.HasTag(_requiredTag));
        }
        
        [Test]
        public void TryActiveAbility_AbilityShouldOnlyActive_WhenSystemHasRequiredTag()
        {
            _abilitySO.Tags.OwnerTags.RequireTags = new TagScriptableObject[] {_requiredTag};
            var ability = _abilitySystem.GiveAbility(_abilitySO);
            _abilitySystem.TryActiveAbility(ability);
            Assert.IsFalse(ability.IsActive);

            _abilitySystem.TagSystem.GrantedTags.Add(_requiredTag);
            _abilitySystem.TryActiveAbility(ability);
            Assert.IsTrue(ability.IsActive);
        }
        
        [Test]
        public void TryActiveAbility_AbilityShouldOnlyActive_WhenSystemHasNoneIgnoreTag()
        {
            _abilitySO.Tags.OwnerTags.IgnoreTags = new TagScriptableObject[] {_ignoredTag};
            var ability = _abilitySystem.GiveAbility(_abilitySO);
            _abilitySystem.TryActiveAbility(ability);
            Assert.IsTrue(ability.IsActive);

            ability.EndAbility();
            _abilitySystem.TagSystem.GrantedTags.Add(_ignoredTag);
            _abilitySystem.TryActiveAbility(ability);
            Assert.IsFalse(ability.IsActive);
        }
        
        [Test]
        public void RemoveAbility_AbilityShouldNotActive_AndRemovedFromGrantedList()
        {
            var ability = _abilitySystem.GiveAbility(_abilitySO);
            _abilitySystem.TryActiveAbility(ability);
            _abilitySystem.RemoveAbility(ability);
            Assert.IsFalse(ability.IsActive);
            Assert.IsFalse(_abilitySystem.GrantedAbilities.Abilities.Contains(ability));
        }
        
        [Test]
        public void RemoveAllAbilities_GrantedListShouldBeEmpty()
        {
            _abilitySO.Tags.ActivationTags = new TagScriptableObject[] {_requiredTag};
            var ability = _abilitySystem.GiveAbility(_abilitySO);
            _abilitySystem.RemoveAllAbilities();
            Assert.IsEmpty(_abilitySystem.GrantedAbilities.Abilities);
            Assert.IsFalse(_abilitySystem.TagSystem.HasTag(_requiredTag));
        }
    }
}
