using CryptoQuest.Gameplay.Battle.Core;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class DamageOverTimeCalculation : EffectExecutionCalculationBase
    {
        [SerializeField] private CustomExecutionAttributeCaptureDef _targetMaxHp;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            var spec = executionParams.EffectSpec;
            var context = GameplayEffectContext.ExtractEffectContext(spec.Context);
            if (context == null)
            {
                Debug.LogWarning("DamageOverTimeCalculation::Execute::context is null" +
                                 "\nThis execution depends on DerivedGameplayEffectContext");
                return;
            }

            SkillParameters skillParameters = context.SkillInfo.SkillParameters;
            CustomExecutionAttributeCaptureDef targetAttribute = skillParameters.targetAttribute;
            executionParams.TryGetAttributeValue(_targetMaxHp, out var targetMaxHp);
            var damageValue = targetMaxHp.CurrentValue / 20;
            var modifier = new GameplayModifierEvaluatedData()
            {
                Attribute = targetAttribute.Attribute,
                ModifierOp = EAttributeModifierOperationType.Add,
                Magnitude = -Mathf.RoundToInt(damageValue)
            };
            outModifiers.Add(modifier);
        }
    }
}