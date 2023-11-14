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
        public class TestCounterPolicy : CounterPolicy
        {
            public Action CounterEvent { get; set; }

            public TestCounterPolicy() : base() {}
            public TestCounterPolicy(int counter) : base(counter) {}

            public override void RegistCounterEvent(CounterGameplayEffect effect)
            {
                base.RegistCounterEvent(effect);
                CounterEvent += effect.ReduceCounterEvent;
            }

            public override void RemoveCounterEvent(CounterGameplayEffect effect)
            {
                base.RemoveCounterEvent(effect);
                CounterEvent -= effect.ReduceCounterEvent;
            }
        }

        [UnityTest]
        public IEnumerator AfterCounter_ShouldRemoveFromSystem()
        {
            var (_, _, effectSystem, _) = CreateAbilitySystem();
            var def = ScriptableObject.CreateInstance<GameplayEffectDefinition>();
            var counter = 5;
            var counterAction = new TestCounterPolicy(counter);
            CounterEvent = counterAction.CounterEvent;
            def.Policy = counterAction;
            var spec = effectSystem.GetEffect(def);
            var activeSpec = effectSystem.ApplyEffectToSelf(spec);
            Assert.IsTrue(activeSpec.IsValid());
            Assert.AreEqual(1, effectSystem.AppliedEffects.Count);
            yield return ReduceCounter(counter);
            Assert.AreEqual(0, effectSystem.AppliedEffects.Count);
            CounterEvent = null;
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