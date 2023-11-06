using CryptoQuest.AbilitySystem.Attributes;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    [CreateAssetMenu(menuName = "A")]
    public class SetFixedValueCalculation : EffectExecutionCalculationBase
    {
        [SerializeField] private float _fixedValue = 1f;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            var context = GameplayEffectContext.ExtractEffectContext(executionParams.EffectSpec.Context);
            var parameters = context.SkillInfo.SkillParameters;
            var targetAttribute = parameters.TargetAttribute.Attribute;

            if (executionParams.TargetAbilitySystemComponent.TryGetComponent(
                    out Battle.Components.Character character) == false) return;

            character.AttributeSystem.TryGetAttributeValue(targetAttribute, out var attributeValue);

            var mag = _fixedValue - attributeValue.CurrentValue;

            var outMod = new GameplayModifierEvaluatedData()
            {
                Attribute = targetAttribute,
                OpType = EAttributeModifierOperationType.Add,
                Magnitude = mag
            };
            
            Debug.Log($"SetFixedValueCalculation:: {parameters.TargetAttribute.Attribute} amount {mag}");
            outModifiers.Add(outMod);
        }
    }
}