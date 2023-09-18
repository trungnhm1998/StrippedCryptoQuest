using System.Collections.Generic;
using CryptoQuest.Gameplay;
using CryptoQuest.Gameplay.Battle.Core;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.Battle.ExecutionCalculations
{
    [CreateAssetMenu(menuName = "Gameplay/Battle/Effects/Execution Calculations/Item Calculation",
        fileName = "Item Calculation")]
    public class ItemCalculation : EffectExecutionCalculationBase
    {
        private const float PERCENTAGE = 100f;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref List<EffectAttributeModifier> outModifiers)
        {
            // TODO: REFACTOR GAS
            // CryptoQuestGameplayEffectSpec effectSpec = (CryptoQuestGameplayEffectSpec)executionParams.EffectSpec;
            // SkillParameters skillParameters = effectSpec.SkillParam;
            // CustomExecutionAttributeCaptureDef targetedAttribute = skillParameters.targetAttribute;
            // AttributeScriptableObject attribute = targetedAttribute.Attribute;
            // EAttributeModifierType modifierType = EAttributeModifierType.Add;
            //
            // float value = skillParameters.BasePower;
            //
            // if (!skillParameters.IsFixed)
            // {
            //     modifierType = EAttributeModifierType.Multiply;
            //     value /= PERCENTAGE;
            // }
            //
            // EffectAttributeModifier modifier = new EffectAttributeModifier()
            // {
            //     Attribute = attribute,
            //     ModifierType = modifierType,
            //     Value = value,
            // };
            //
            // outModifiers.Add(modifier);
        }
    }
}