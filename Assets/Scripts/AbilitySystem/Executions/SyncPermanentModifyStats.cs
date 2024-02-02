using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Sagas.Items;
using IndiGames.Core.Events;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    /// <summary>
    /// Get modify info from effect details and sync hero info to server
    /// </summary>
    public class SyncPermanentModifyStats : EffectExecutionCalculationBase
    {
        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            if (executionParams.TargetAbilitySystemComponent.TryGetComponent(
                out HeroBehaviour hero) == false) return;

            List<AttributeWithValue> attributes = new();
            foreach (var mod in executionParams.EffectSpec.EffectDefDetails.Modifiers)
            {
                attributes.Add(new AttributeWithValue(mod.Attribute, mod.Value));
            }

            ActionDispatcher.Dispatch(new SyncStatsAction(hero, attributes.ToArray()));
        }
    }
}