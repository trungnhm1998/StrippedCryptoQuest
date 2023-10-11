using CryptoQuest.AbilitySystem.Attributes;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using IndiGames.GameplayAbilitySystem.TagSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class GodMode : EffectExecutionCalculationBase
    {
        [SerializeField] private TagScriptableObject _godModeTag;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            if (executionParams.TargetAbilitySystemComponent.TagSystem.HasTag(_godModeTag) == false) return;
            // find damage modifier
            // var damageModifier = outModifiers.GetModifier<float>(ExecutionCalculationConstants.DamageModifier);
            for (var index = 0; index < outModifiers.Modifiers.Count; index++)
            {
                var modifier = outModifiers.Modifiers[index];
                if (modifier.Attribute != AttributeSets.Health) continue;
                modifier.Magnitude = 0;
                outModifiers.Modifiers[index] = modifier;
            }
        }
    }
}