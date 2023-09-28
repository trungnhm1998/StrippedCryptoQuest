using System;
using System.Collections;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace IndiGames.GameplayAbilitySystemTests
{
    public class CounterEffectTests : FixtureBase
    {
        [UnityTest]
        public IEnumerator AfterCounter_ShouldRemoveFromSystem()
        {
            var (_, _, effectSystem, _) = CreateAbilitySystem();
            var def = ScriptableObject.CreateInstance<GameplayEffectDefinition>();
            var counter = 5;
            var counterAction = new CounterAction(counter);
            CounterEvent += CounterEffectSpecification.ReduceCounterEvent;
            def.EffectAction = counterAction;
            var spec = effectSystem.GetEffect(def);
            var activeSpec = effectSystem.ApplyEffectToSelf(spec);
            Assert.IsTrue(activeSpec.IsValid());
            Assert.AreEqual(1, effectSystem.AppliedEffects.Count);
            yield return ReduceCounter(counter);
            Assert.AreEqual(0, effectSystem.AppliedEffects.Count);
            CounterEvent -= CounterEffectSpecification.ReduceCounterEvent;
        }

        private Action CounterEvent;
        private IEnumerator ReduceCounter(int counter)
        {
            while (counter > 0)
            {
                counter--;
                CounterEvent?.Invoke();
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}