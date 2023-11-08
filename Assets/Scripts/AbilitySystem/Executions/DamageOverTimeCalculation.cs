using CryptoQuest.Gameplay.Battle.Core;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class DamageOverTimeCalculation : EffectExecutionCalculationBase
    {
        [SerializeField] private CustomExecutionAttributeCaptureDef _targetMaxHp;
        [SerializeField] private float _damageToAttributeRatio = 0.05f;

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
            CustomExecutionAttributeCaptureDef targetAttribute = skillParameters.TargetAttribute;
            executionParams.TryGetAttributeValue(_targetMaxHp, out var targetMaxHp);
            var damageValue = targetMaxHp.CurrentValue * _damageToAttributeRatio;
            // specs: https://docs.google.com/spreadsheets/d/1EDrXrT1if63TLam1km_dLx2VvHt2rks2OWXEUq5OEPg/edit#gid=395250385
            var modifier = new GameplayModifierEvaluatedData()
            {
                Attribute = targetAttribute.Attribute,
                OpType = EAttributeModifierOperationType.Add,
                Magnitude = -Mathf.RoundToInt(damageValue)
            };
            outModifiers.Add(modifier);
        }
    }
}