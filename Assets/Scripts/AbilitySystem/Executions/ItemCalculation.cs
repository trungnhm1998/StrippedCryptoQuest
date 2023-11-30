using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    [CreateAssetMenu(menuName = "Gameplay/Battle/Effects/Execution Calculations/Item Calculation",
        fileName = "Item Calculation")]
    public class ItemCalculation : EffectExecutionCalculationBase
    {
        private const float PERCENTAGE = 100f;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            var spec = executionParams.EffectSpec;
            var context = GameplayEffectContext.ExtractEffectContext(spec.Context);
            if (context == null)
            {
                Debug.LogWarning("ItemCalculation::Execute::context is null" +
                                 "\nThis execution depends on DerivedGameplayEffectContext");
                return;
            }

            SkillParameters skillParameters = context.SkillInfo.SkillParameters;
            CustomExecutionAttributeCaptureDef targetedAttribute = skillParameters.TargetAttribute;
            AttributeScriptableObject attribute = targetedAttribute.Attribute;
            EAttributeModifierOperationType modifierOperationType = EAttributeModifierOperationType.Add;

            float value = skillParameters.BasePower;

            if (!skillParameters.IsFixed)
            {
                modifierOperationType = EAttributeModifierOperationType.Multiply;
                value /= PERCENTAGE;
            }

            var modifier = new GameplayModifierEvaluatedData()
            {
                Attribute = attribute,
                OpType = modifierOperationType,
                Magnitude = Mathf.RoundToInt(value)
            };

            outModifiers.Add(modifier);
        }
    }
}