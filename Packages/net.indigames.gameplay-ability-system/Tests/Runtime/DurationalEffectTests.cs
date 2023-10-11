using System.Collections;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace IndiGames.GameplayAbilitySystemTests
{
    public class DurationalEffectTests : FixtureBase
    {
        [UnityTest]
        public IEnumerator AfterDuration_ShouldRemoveFromSystem()
        {
            var (_, _, effectSystem, _) = CreateAbilitySystem();
            var def = ScriptableObject.CreateInstance<GameplayEffectDefinition>();
            var duration = 1f;
            def.Policy = new DurationalPolicy(duration);
            var spec = effectSystem.GetEffect(def);
            var activeSpec = effectSystem.ApplyEffectToSelf(spec);
            Assert.IsTrue(activeSpec.IsValid());
            Assert.AreEqual(1, effectSystem.AppliedEffects.Count);
            yield return new WaitForSeconds(duration);
            Assert.AreEqual(0, effectSystem.AppliedEffects.Count);
        }
    }
}