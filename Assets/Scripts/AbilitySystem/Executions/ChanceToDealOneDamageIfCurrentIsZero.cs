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
            var outMod = outModifiers.Modifiers.FirstOrDefault(mod => mod.Attribute == AttributeSets.Health);
            if (Mathf.Approximately(outMod.Magnitude, 0f) == false) return;
            if (Random.Range(0f, 1f) > _chance) return;
            outMod.Magnitude = 1f;
        }
    }
}