using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem.Components;
using NUnit.Framework;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystemTests
{
    public class FixtureBase
    {
        private List<GameObject> _gameObjectsToDestroy = new List<GameObject>();

        internal (TagSystemBehaviour, AttributeSystemBehaviour, EffectSystemBehaviour, AbilitySystemBehaviour)
            CreateAbilitySystem()
        {
            var go = new GameObject();
            _gameObjectsToDestroy.Add(go);

            var tagSystem = go.AddComponent<TagSystemBehaviour>();
            var attributeSystem = go.AddComponent<AttributeSystemBehaviour>();
            var effectSystem = go.AddComponent<EffectSystemBehaviour>();
            var abilitySystem = go.AddComponent<AbilitySystemBehaviour>();
            return (tagSystem, attributeSystem, effectSystem, abilitySystem);
        }

        [TearDown]
        public void Teardown()
        {
            foreach (var go in _gameObjectsToDestroy)
            {
                Object.DestroyImmediate(go);
            }
        }
    }
}