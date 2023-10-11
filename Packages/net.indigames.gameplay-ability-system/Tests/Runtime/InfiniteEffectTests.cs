using System.Collections;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace IndiGames.GameplayAbilitySystemTests
{
    public class InfiniteEffectTests : FixtureBase
    {
        [UnityTest]
        public IEnumerator IsActive_False_ShouldRemoveFromSystem()
        {
            var (_, _, effectSystem, _) = CreateAbilitySystem();
            var def = ScriptableObject.CreateInstance<GameplayEffectDefinition>();
            def.Policy = new InfinitePolicy();
            var spec = effectSystem.GetEffect(def);
            var activeSpec = effectSystem.ApplyEffectToSelf(spec);
            Assert.IsTrue(activeSpec.IsValid());
            yield return null;
            Assert.AreEqual(1, effectSystem.AppliedEffects.Count);
            yield return null;
            activeSpec.IsActive = false;
            yield return null;
            Assert.AreEqual(0, effectSystem.AppliedEffects.Count);
        }
    }
}