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
            def.EffectAction = new InfiniteAction();
            var spec = effectSystem.GetEffect(def);
            var activeSpec = effectSystem.ApplyEffectToSelf(spec);
            Assert.IsTrue(activeSpec.IsValid());
            yield return null;
            Assert.AreEqual(effectSystem.AppliedEffects.Count, 1);
            yield return null;
            activeSpec.IsActive = false;
            yield return null;
            Assert.AreEqual(effectSystem.AppliedEffects.Count, 0);
        }
    }

    public class DurationalEffectTests : FixtureBase
    {
        [UnityTest]
        public IEnumerator AfterDuration_ShouldRemoveFromSystem()
        {
            var (_, _, effectSystem, _) = CreateAbilitySystem();
            var def = ScriptableObject.CreateInstance<GameplayEffectDefinition>();
            var duration = 1f;
            def.EffectAction = new DurationalAction(duration);
            Assert.AreEqual(effectSystem.AppliedEffects.Count, 1);
            yield return new WaitForSeconds(duration);
            Assert.AreEqual(effectSystem.AppliedEffects.Count, 0);
        }
    }
}