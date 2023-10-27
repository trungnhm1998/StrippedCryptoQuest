using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class MagicCalculation : EffectExecutionCalculationBase
    {
        [SerializeField] private CustomExecutionAttributeCaptureDef _baseMagicAttack;
        [SerializeField] private float _lowerRandomRange = -0.05f;
        [SerializeField] private float _upperRandomRange = 0.05f;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            var spec = executionParams.EffectSpec;
            var context = GameplayEffectContext.ExtractEffectContext(spec.Context);
            var parameters = context.SkillInfo.SkillParameters;
            executionParams.TryGetAttributeValue(_baseMagicAttack, out var baseMagicAttack);
            Debug.Log($"Calculation::Magic atk: {baseMagicAttack.CurrentValue}");
            var targetedAttributeDef = parameters.targetAttribute;
            float baseMagicValue =
                BattleCalculator.CalculateMagicSkillBasePower(parameters, baseMagicAttack.CurrentValue);
            baseMagicValue.Offset(Random.Range(_lowerRandomRange, _upperRandomRange));

            Debug.Log($"Calculation::Magic Power: {baseMagicValue}");

            if (baseMagicValue <= 0f) return;

            var modifier = new GameplayModifierEvaluatedData()
            {
                Attribute = targetedAttributeDef.Attribute,
                Magnitude = Mathf.RoundToInt(baseMagicValue),
                OpType = EAttributeModifierOperationType.Add,
            };
            outModifiers.Add(modifier);
        }
    }
}