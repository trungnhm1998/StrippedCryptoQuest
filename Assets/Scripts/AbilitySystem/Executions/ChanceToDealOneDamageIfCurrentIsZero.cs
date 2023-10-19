using System;
using System.Linq;
using CryptoQuest.AbilitySystem.Attributes;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class ChanceToDealOneDamageIfCurrentIsZero : EffectExecutionCalculationBase
    {
        [SerializeField] private float _chance = 0.5f;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            try
            {
                var outMod = outModifiers.Modifiers.FirstOrDefault(mod => mod.Attribute == AttributeSets.Health);
                if (Mathf.Approximately(outMod.Magnitude, 0f) == false) return;
                if (UnityEngine.Random.Range(0f, 1f) > _chance) return;
                outMod.Magnitude = 1f;
            }
            catch (Exception e)
            {
                Debug.Log($"Failed to get Health attribute from outModifiers: {e.Message}");
            }
        }
    }
}