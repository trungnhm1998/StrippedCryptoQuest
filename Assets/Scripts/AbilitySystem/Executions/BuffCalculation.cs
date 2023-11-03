using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class BuffCalculation : EffectExecutionCalculationBase
    {
        private enum StatusType
        {
            Buff = 1,
            Debuff = -1
        }

        [SerializeField] private StatusType _statusType;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            var context = GameplayEffectContext.ExtractEffectContext(executionParams.EffectSpec.Context);
            var parameters = context.SkillInfo.SkillParameters;

            for (var index = 0; index < outModifiers.Modifiers.Count; index++)
            {
                var outMod = outModifiers.Modifiers[index];
                if (outMod.Attribute != parameters.TargetAttribute.Attribute) continue;
                outMod.Magnitude /= 100f * (int)_statusType;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                var sign = _statusType == StatusType.Buff ? "+" : "-";
                var type = parameters.IsFixed ? "" : "%";
                var isIncrease = _statusType == StatusType.Buff ? "increase" : "decrease";
                Debug.Log($"Calculation::Buff {outMod.Attribute.name} {isIncrease} {sign}{Mathf.Abs(outMod.Magnitude)}{type}");
#endif
                outMod.OpType = parameters.IsFixed
                    ? EAttributeModifierOperationType.Add
                    : EAttributeModifierOperationType.Multiply;
                outModifiers.Modifiers[index] = outMod;
            }
        }
    }
}