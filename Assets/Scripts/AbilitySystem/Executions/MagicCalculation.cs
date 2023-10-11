using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    [CreateAssetMenu(fileName = "HealCalculation",
        menuName = "Gameplay/Battle/Effects/Execution Calculations/Heal Calculation")]
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
            var targetedAttributeDef = parameters.targetAttribute;
            float baseMagicValue = BattleCalculator.CalculateBaseDamage(parameters,
                baseMagicAttack.CurrentValue,
                Random.Range(_lowerRandomRange, _upperRandomRange));

            Debug.Log($"Magic value: {baseMagicAttack}");

            if (baseMagicValue <= 0f) return;

            var modifier = new GameplayModifierEvaluatedData()
            {
                Attribute = targetedAttributeDef.Attribute,
                // TODO: Refactor this when implement buff effect
                Magnitude = Mathf.RoundToInt(baseMagicValue) *
                            (context.Parameters.EffectType == EEffectType.Restore ? 1 : -1),
                ModifierOp = EAttributeModifierOperationType.Add,
            };
            outModifiers.Add(modifier);
        }
    }
}