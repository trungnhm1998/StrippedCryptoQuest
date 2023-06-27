using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Indigames.AbilitySystem;

namespace Indigames.AbilitySystem.Tests.Ability
{
    public class AbilitySystemBehaviourTests
    {
        [Test]
        public void AbilitySystem_OnValidateAssignComponentsCorrectly()
        {
            var abilityOwner = new GameObject();
            var abilitySystem = abilityOwner.AddComponent<AbilitySystemBehaviour>();
            
            Assert.IsNotNull(abilitySystem.AttributeSystem, $"{abilityOwner} has no Attribute System.");
            Assert.IsNotNull(abilitySystem.TagSystem, $"{abilityOwner} has no Tag System.");
            Assert.IsNotNull(abilitySystem.EffectSystem, $"{abilityOwner} has no Effect System.");
        }
    }
}
